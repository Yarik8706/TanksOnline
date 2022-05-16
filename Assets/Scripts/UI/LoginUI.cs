using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace UI
{
    public class LoginUI : MonoBehaviour
    {
        private static string _playerName;
        public static string playerName
        {
            get => string.IsNullOrEmpty(_playerName) ? SetPlayerNameIpAddress() : _playerName;
            private set => _playerName = value;
        }
        
        private static string SetPlayerNameIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork || !ip.ToString().Contains("192.168.")) continue;
                return ip.ToString();
            }

            return "Name";
        }

        public void SetPlayerName(string value)
        {
            playerName = value;
        }
    }
}