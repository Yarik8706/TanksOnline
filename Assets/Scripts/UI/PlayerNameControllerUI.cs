using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace UI
{
    public class PlayerNameControllerUI : MonoBehaviour
    {
        private static string _playerName;
        public static string playerName
        {
            get => string.IsNullOrEmpty(_playerName) ? SetBaseName() : _playerName;
            private set => _playerName = value;
        }
        
        private static string SetBaseName()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "Name";
        }

        public void SetName(string value)
        {
            playerName = value;
        }
    }
}