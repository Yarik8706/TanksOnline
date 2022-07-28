using System.Collections.Generic;
using Gameplay;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class KillCountsStatsUI : MonoBehaviour
    {
        public Text textPrefab;
        public readonly Dictionary<string, Text> playerStats = new Dictionary<string, Text>();

        public void AddPlayer(string namePlayer)
        {
            var textObject = Instantiate(textPrefab, gameObject.transform, true);
            var text = textObject.GetComponent<Text>();
            playerStats.Add(namePlayer, text);
            text.text = namePlayer + " : " + 0;
        }

        public void UpdatePlayersStats(SyncDictionary<string, int> data)
        {
            List<string> keys = new List<string>();
            foreach (var key in playerStats.Keys)
            {
                if (data.ContainsKey(key))
                {
                    playerStats[key].text = key + " : " + data[key];
                }
                else
                {
                    keys.Add(key);
                }
            }

            foreach (var key in keys)
            {
                Destroy(playerStats[key].gameObject);
                playerStats.Remove(key);
            }
            keys.Clear();

            foreach (var dataKey in data.Keys)
            {
                if (!playerStats.ContainsKey(dataKey))
                {
                    keys.Add(dataKey);
                }
            }

            foreach (var key in keys)
            {
                AddPlayer(key);
            }
        }

        public void UpdatePlayerName(string oldName, string newName)
        {
            var stats = playerStats[oldName];
            playerStats.Remove(oldName);
            playerStats.Add(newName, stats);
            stats.text = stats.text.Replace(oldName, newName);
        }

        public void DeletePlayerStats(string key)
        {
            if(!playerStats.ContainsKey(key)) return;
            Destroy(playerStats[key].gameObject);
            playerStats.Remove(key);
        }

        public void ClearAll()
        {
            foreach (var text in playerStats.Values)
            {
                Destroy(text.gameObject);
            }
            playerStats.Clear();
        }
    }
}