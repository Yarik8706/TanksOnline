using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class KillCountsStatsUI : MonoBehaviour
    {
        public Text textPrefab;
        public readonly Dictionary<string, Text> playerStats = new Dictionary<string, Text>();

        public void AddPlayerStats(string namePlayer, int stats)
        {
            var textObject = Instantiate(textPrefab, gameObject.transform, true);
            var text = textObject.GetComponent<Text>();
            playerStats.Add(namePlayer, text);
            text.text = namePlayer + " : " + stats;
        }

        public void UpdatePlayerStats(string playerName, int newStat)
        {
            playerStats[playerName].text = playerName + " : " + newStat;
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