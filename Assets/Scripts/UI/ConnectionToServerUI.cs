using System;
using Gameplay;
using kcp2k;
using UnityEngine;

namespace UI
{
    public class ConnectionToServerUI : MonoBehaviour
    {
        [SerializeField] private CustomNetworkManager customNetworkManager;
        [SerializeField] private KcpTransport kcpTransport;
        private string _ipaddress;
        private string _port;

        public void OnChangeIpaddess(string value)
        {
            _ipaddress = value;
        }

        public void OnChangePost(string value)
        {
            _port = value;
        }

        public void StartConnection()
        {
            customNetworkManager.networkAddress = _ipaddress;
            kcpTransport.Port = Convert.ToUInt16(_port);
            customNetworkManager.StartClient();
        }
    }
}