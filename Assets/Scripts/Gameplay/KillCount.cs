using System;
using Mirror;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class KillCount : NetworkBehaviour
    {
        public readonly SyncDictionary<string, int> killStats = new SyncDictionary<string, int>();
        private KillCountsStatsUI _killCountsStatsUI;

        private void Awake()
        {
            _killCountsStatsUI = FindObjectOfType<KillCountsStatsUI>();
        }

        [ServerCallback]
        public void AddPlayerStats(string playerName)
        {
            killStats.Add(playerName, 0);
        }

        private void Update()
        {
            UpdateDataForPlayer();
        }

        private void UpdateDataForPlayer()
        {
            if(!isLocalPlayer) return;
            _killCountsStatsUI.UpdatePlayersStats(killStats);
        }

        [ServerCallback]
        public void DeletePlayerStats(string playerName)
        {
            killStats.Remove(playerName);
        }
    }
}