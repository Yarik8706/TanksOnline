using System.Collections.Generic;
using Gameplay;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public struct KillStats
    {
        public string playerName;
        public int killCount;

        public KillStats(string playerName, int killCount)
        {
            this.killCount = killCount;
            this.playerName = playerName;
        }
    }
        
    public class KillCountsStatsUI : NetworkBehaviour
    {
        public Text textPrefab;
        private readonly Dictionary<string, Text> _playerStats = new Dictionary<string, Text>();

        public void AddPlayer(string namePlayer)
        {
            var textObject = Instantiate(textPrefab, gameObject.transform, true);
            var text = textObject.GetComponent<Text>();
            _playerStats.Add(namePlayer, text);
            text.text = namePlayer + " : " + 0;
        }

        public void UpdatePlayerStats(string namePlayer, int count = 0)
        {
            _playerStats[namePlayer].text = namePlayer + " : " + count;
        }

        public void UpdatePlayerName(string oldName, string newName)
        {
            var stats = _playerStats[oldName];
            _playerStats.Remove(oldName);
            _playerStats.Add(newName, stats);
            stats.text = stats.text.Replace(oldName, newName);
        }

        public void DeletePlayerStats(string key)
        {
            if(!_playerStats.ContainsKey(key)) return;
            Destroy(_playerStats[key].gameObject);
            _playerStats.Remove(key);
        }

        public bool HasKey(string key)
        {
            return _playerStats.ContainsKey(key);
        }

        public void ClearAll()
        {
            foreach (var text in _playerStats.Values)
            {
                Destroy(text.gameObject);
            }
            _playerStats.Clear();
        }
    }
}