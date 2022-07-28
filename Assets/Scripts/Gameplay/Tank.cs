using System;
using System.Collections;
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
        public Animator animator;
        public TextMesh healthBar;
        public TextMesh playerNameBar;
        public GameObject playerStats;
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
        public string playerName;
        [SyncVar] public int health = 4;
        [SyncVar] public bool bodyLampOn = true;
        [SyncVar] public bool turretLampOn = true;
        
        private Rigidbody _rigidbody3D;
        private BoxCollider _boxCollider;
        internal static Tank localTank;
        internal static Text speedAttackTimeText;
        internal KillCount killCountStats;
        private float _speedAttackTime;
        private bool _canRotationTurret = true;
        private TimeToRespawnUI _timeToRespawnUI;
        private Camera _mainCamera;
        
        private void Awake()
        {
            _rigidbody3D = GetComponent<Rigidbody>();
            _boxCollider = GetComponent<BoxCollider>();
            killCountStats = GetComponent<KillCount>();
            _mainCamera = Camera.main;
            if (!isLocalPlayer) return;
            localTank = this;
        }

        private void Start()
        {
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
            _rigidbody3D.velocity = nextPosition * (movementSpeed * Time.deltaTime * 50);
            animator.SetBool("Moving", nextPosition != Vector3.zero);
        
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

        [Command]
        private void SetPlayerName(string newName)
        {
            playerName = newName; 
            killCountStats.AddPlayerStats(playerName);
        }
    
        [ClientRpc]
        private void AttackSync()
        {
            attackShound.Play();
            animator.SetTrigger("Shoot");
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (!(other.GetComponent<Bullet>() is {} bullet)) return;
            --health;
            if (health != 0) return;
            if (bullet.creater != null)
            {
                Dead(bullet.creater);
                return;
            }
            Dead();
        }

        [ServerCallback]
        private void Dead()
        {
            var nextPosition = GameManager.gameManager.startPositions[
                Random.Range(0, GameManager.gameManager.startPositions.Length)].position;
            health = 4;
            DeathSync();
            NetworkServer.Spawn(Instantiate(deadEffect, transform.position, Quaternion.identity));
            ChangeComponentStateInDied(false);
            StartCoroutine(RespawnCoroutine(nextPosition));
        }
        
        [ServerCallback]
        private void Dead(GameObject bulletCreator)
        {
            bulletCreator.GetComponent<KillCount>().killStats[playerName]++;
            var newPikupHealth = Instantiate(pikupHealth, transform.position, Quaternion.identity);
            NetworkServer.Spawn(newPikupHealth);
            Dead();
        }

        [ClientRpc]
        private void DeathSync()
        {
            if (isLocalPlayer) _timeToRespawnUI.StartCounting(respawnTime);
            ChangeComponentStateInDied(false);
            turret.rotation = new Quaternion(0,0,0,0);
            transform.rotation = new Quaternion(0,0,0,0);
        }

        [Server]
        private IEnumerator RespawnCoroutine(Vector3 nextPosition)
        {
            yield return new WaitForSeconds(respawnTime);
            transform.position = nextPosition;
            RespawnSync();
            ChangeComponentStateInDied(true);
        }

        private void ChangeComponentStateInDied(bool state)
        {
            _boxCollider.enabled = state;
            _rigidbody3D.isKinematic = !state;
            animator.gameObject.SetActive(state);
            playerStats.SetActive(state);
            if(isLocalPlayer) SwitchCameras(!state);
        }

        [ClientRpc]
        private void RespawnSync()
        {
            ChangeComponentStateInDied(true);
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