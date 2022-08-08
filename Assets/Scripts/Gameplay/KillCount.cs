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
        
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            _killCountsStatsUI = FindObjectOfType<KillCountsStatsUI>();
            killStats.Callback += UpdateStats;
        }

        [ServerCallback]
        public void AddPlayerStats(string playerName)
        {
            killStats.Add(playerName, 0);
        }

        public void SyncPlayersStats()
        {
            foreach (var killStatsKey in killStats.Keys)
            {
                if(killStatsKey == Tank.localTank.playerName) continue;
                _killCountsStatsUI.AddPlayerStats(killStatsKey, killStats[killStatsKey]);
            }
        }

        [ServerCallback]
        public void DeletePlayerStats(string playerName)
        {
            killStats.Remove(playerName);
        }

        public void UpdateStats(SyncIDictionary<string, int>.Operation op, string key, int item)
        {
            Debug.Log("VAR");
            switch (op) 
            {
                case SyncIDictionary<string, int>.Operation.OP_ADD:
                    _killCountsStatsUI.AddPlayerStats(key, 0);
                    break;
                case SyncIDictionary<string, int>.Operation.OP_CLEAR:
                    _killCountsStatsUI.ClearAll();
                    break;
                case SyncIDictionary<string, int>.Operation.OP_REMOVE:
                    _killCountsStatsUI.DeletePlayerStats(key);
                    break;
                case SyncIDictionary<string, int>.Operation.OP_SET:
                    _killCountsStatsUI.UpdatePlayerStats(key, item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }
    }
}