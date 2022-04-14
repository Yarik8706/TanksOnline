using System;
using Mirror;
using UnityEngine;

public class KillCount : NetworkBehaviour
{
    [SyncVar (hook = "ChangedKillCount")] internal int killCount;
    private Tank _playerObject;
    private KillCountsStatsUI _killCountsStatsUI;

    private void Awake()
    {
        _killCountsStatsUI = FindObjectOfType<KillCountsStatsUI>();
    }

    public void Start()
    {
        _playerObject = GetComponent<Tank>();
    }

    public void AddPlayerStats(string playerName)
    {
        _killCountsStatsUI.AddPlayer(playerName);
    }
    
    public void DeletePlayerStats(string playerName)
    {
        _killCountsStatsUI.DeletePlayerStats(playerName);
    }

    private void ChangedKillCount(int oldValue, int value)
    {
       _killCountsStatsUI.UpdatePlayerStats(_playerObject.playerName, value);
    }

    public void ChangedPlayerName(string oldValue, string value)
    {
        _killCountsStatsUI.UpdatePlayerName(oldValue, value);
    }
}