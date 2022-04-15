using System;
using System.Net;
using System.Net.Sockets;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tank : NetworkBehaviour
{
    [Header("Components")]
    public Animator animator;
    public TextMesh healthBar;
    public TextMesh playerNameBar;
    public GameObject playerStats;
    public GameObject deadController;
    public Transform turret;
    public GameObject turretLamp;
    public GameObject[] bodyLamps;

    [Header("Movement")]
    public float rotationSpeed = 100;
    public float movementSpeed = 5;

    [Header("Firing")]
    public GameObject bulletPrefab;
    public Transform bullerSpawn;
    public float speedAttack = 2;

    [Header("Stats")] 
    [SyncVar (hook = "ChangedPlayerName")] public string playerName;
    [SyncVar] public int health = 4;
    [SyncVar] public bool bodyLampOn = true;
    [SyncVar] public bool turretLampOn = true;

    internal static Tank localPlayerTank;
    internal static Text speedAttackTimeText;
    private float _speedAttackTime;
    private bool _canRotationTurret = true;
    private KillCount _killCountStats;

    private void Awake()
    {
        _killCountStats = GetComponent<KillCount>();
    }

    private void Start()
    {
        _speedAttackTime = speedAttack;
        if (!isLocalPlayer) return;
        StartSetPlayerName();
        localPlayerTank = this;
    }

    private void StartSetPlayerName()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily != AddressFamily.InterNetwork) continue;
            Debug.Log(ip.ToString());
            SetPlayerName(ip.ToString());
        }
        var inputPlayerName = LoginUI.namePlayer;
        if (!string.IsNullOrEmpty(inputPlayerName))
        {
            SetPlayerName(inputPlayerName);
        }
    }

    private void Update()
    {
        healthBar.text = new string('-', health);
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
        transform.position += nextPosition * (movementSpeed * Time.deltaTime);
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
        AttackAnimation();
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
        playerStats.gameObject.SetActive(bodyLampOn);
        if (localPlayerTank == this)
        {
            playerStats.gameObject.SetActive(true);
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
        playerStats.gameObject.SetActive(turretLampOn);
        if (localPlayerTank == this)
        {
            playerStats.gameObject.SetActive(true);
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
    private void AttackAnimation()
    {
        animator.SetTrigger("Shoot");
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (bullet == null) return;
        --health;
        if (health != 0) return;
        Dead();
        bullet.creater.GetComponent<KillCount>().killCount++;
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

    [ClientRpc]
    private void DeathSync(Vector3 nextPosition)
    {
        if(isLocalPlayer) GameManager.respawnTime.SetActive(true);
        turret.rotation = new Quaternion(0,0,0,0);
        transform.rotation = new Quaternion(0,0,0,0);
        CreateDeadController(nextPosition);
    }

    private void CreateDeadController(Vector3 nextPosition)
    {
        var controller = Instantiate(deadController);
        controller.GetComponent<DeadController>().Active(gameObject, nextPosition);
    }
    
    private void RotateTurretUpdate()
    {
        var positionLook = InputHandler.GetTurretRotation(transform.position);
        if (positionLook == transform.position || !_canRotationTurret)
        {
            return;
        }
        positionLook.y = turret.position.y;
        turret.LookAt(positionLook);
    }
}