using System;
using Mirror;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class Tank : NetworkBehaviour
    {
        [Header("Components")]
        public TextMesh healthBar;
        public TextMesh playerNameBar;
        public GameObject playerStats;
        public GameObject deadController;
        public Transform turret;
        public GameObject turretLamp;
        public GameObject[] bodyLamps;
        public AudioSource attackShound;
        public GameObject deadEffect;
        public Camera playerCamera;
        public Material outlinedMaterial;
        public SkinnedMeshRenderer tankMeshRenderer;
        public GameObject pikupHealth;

        [Header("Movement")]
        public float rotationSpeed = 100;
        public float movementSpeed = 5;

        [Header("Attack")]
        public GameObject bulletPrefab;
        public Transform bullerSpawn;
        public float speedAttack = 2;

        [Header("Stats")] 
        public float respawnTime = 5;
        [SyncVar (hook = nameof(ChangedPlayerName))] public string playerName;
        [SyncVar] public int health = 4;
        [SyncVar] public bool bodyLampOn = true;
        [SyncVar] public bool turretLampOn = true;

        internal static Tank localTank;
        internal static Text speedAttackTimeText;
        private float _speedAttackTime;
        private bool _canRotationTurret = true;
        private KillCount _killCountStats;
        private TimeToRespawnUI _timeToRespawnUI;
        private Camera _mainCamera;
        private BoxCollider _boxCollider;
        private Rigidbody _rigidbody; 
        private Animator _animator;

        private void Awake()
        {
            _killCountStats = GetComponent<KillCount>();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _boxCollider = GetComponent<BoxCollider>();
            _timeToRespawnUI = FindObjectOfType<TimeToRespawnUI>();
            _speedAttackTime = speedAttack;
            if (!isLocalPlayer) return;
            SetPlayerName(LoginUI.playerName);
            SwitchCameras(false);
            localTank = this;
            healthBar.gameObject.SetActive(false);
            playerNameBar.gameObject.SetActive(false);
            tankMeshRenderer.materials = new[] {outlinedMaterial};
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                healthBar.text = new string('-', health);
            }
            CheckingBodyLampsState();
            CheckingTurretLampsState();
            if (transform.position.y < -10)
            {
                Dead();
            }
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
        
            var horizontal = InputHandler.GetHorizontalMovementChanges();
            transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
        
            var vertical = InputHandler.GetVerticalMovementChanges();
            var nextPosition = transform.forward * vertical;
            _rigidbody.velocity = nextPosition * (movementSpeed * Time.deltaTime * 100);
            _animator.SetBool("Moving", nextPosition != Vector3.zero);
        
            RotateTurretUpdate();
            AttackUpdate();
        }

        private void AttackUpdate()
        {
            _speedAttackTime -= Time.deltaTime;
            if (speedAttack - _speedAttackTime > 0.4)
            {
                _canRotationTurret = true;
            }
            if (_speedAttackTime < 0)
            {
                speedAttackTimeText.text = "Огонь";
            }
            else
            {
                speedAttackTimeText.text = "" + Math.Round(_speedAttackTime, 1);
            }
        }

        public void SwitchCameras(bool enabledMain)
        {
            if(!isLocalPlayer) return;
            playerCamera.enabled = !enabledMain;
            _mainCamera.enabled = enabledMain;
        }

        public void Attack()
        {
            if (!(_speedAttackTime <= 0)) return;
            CommandSpawnBullet();
            _canRotationTurret = false;
            _speedAttackTime = speedAttack;
        }

        // this is called on the server
        [Command]
        private void CommandSpawnBullet()
        {
            var bullet = Instantiate(bulletPrefab, bullerSpawn.position, bullerSpawn.rotation);
            bullet.GetComponent<Bullet>().creater = gameObject;
            NetworkServer.Spawn(bullet);
            AttackSync();
        }

        [Command]
        public void ChangeBodyLampState()
        {
            bodyLampOn = !bodyLampOn;
        }

        private void CheckingBodyLampsState()
        {
            foreach (var lamp in bodyLamps) 
            {
                lamp.SetActive(bodyLampOn);
            }
            playerStats.SetActive(bodyLampOn);
            if (localTank == this)
            {
                playerStats.SetActive(true);
            }
        }

        [Command]
        public void ChangeTurretLampState()
        {
            turretLampOn = !turretLampOn;
        }

        private void CheckingTurretLampsState()
        {
            turretLamp.SetActive(turretLampOn);
            playerStats.SetActive(turretLampOn);
            if (localTank == this)
            {
                playerStats.SetActive(true);
            }
        }

        private void ChangedPlayerName(string oldName, string value)
        {
            playerNameBar.text = value;
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrWhiteSpace(oldName))
            {
                _killCountStats.AddPlayerStats(value);
            }
            else
            {
                _killCountStats.ChangedPlayerName(oldName, value);
            }
        }
    
        [Command]
        private void SetPlayerName(string newName)
        {
            playerName = newName;
        }
    
        [ClientRpc]
        private void AttackSync()
        {
            attackShound.Play();
            _animator.SetTrigger("Shoot");
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet == null) return;
            --health;
            if (health != 0) return;
            if (bullet.creater != null) bullet.creater.GetComponent<KillCount>().killCount++;
            _boxCollider.enabled = false;
            var newPikupHealth = Instantiate(pikupHealth, transform.position, Quaternion.identity);
            NetworkServer.Spawn(newPikupHealth);
            Dead();
        }
 
        [Server]
        private void Dead()
        {
            health = 4;
            var nextPosition = GameManager.gameManager.startPositions[
                Random.Range(0, GameManager.gameManager.startPositions.Length)].position;
            DeathSync(nextPosition);
            CreateDeadController(nextPosition);
        }

        private void OnEnable()
        {
            if (!isLocalPlayer) return;
            _boxCollider.enabled = true;
            SwitchCameras(false);
        }

        [ClientRpc]
        private void DeathSync(Vector3 nextPosition)
        {
            if (isLocalPlayer)
            {
                _timeToRespawnUI.StartCounting(respawnTime);
                SwitchCameras(true);
            }
            turret.rotation = new Quaternion(0,0,0,0);
            transform.rotation = new Quaternion(0,0,0,0);
            CreateDeadController(nextPosition);
        }

        private void CreateDeadController(Vector3 nextPosition)
        {
            Instantiate(deadEffect, transform.position, Quaternion.identity);
            var controller = Instantiate(deadController);
            controller.GetComponent<DeadController>().Active(gameObject, nextPosition, respawnTime);
        }
    
        private void RotateTurretUpdate()
        {
            var rotationValues = InputHandler.GetTurretRotation();
            var positionLook = transform.forward * rotationValues.z + transform.right * rotationValues.x;
            if (positionLook == Vector3.zero || !_canRotationTurret)
            {
                return;
            }
            positionLook += transform.position;
            positionLook.y = transform.position.y;
            turret.LookAt(positionLook);
            turret.rotation = new Quaternion(0, turret.rotation.y, 0, turret.rotation.w);
        }
    }
}