using System;
using Gameplay;
using Mirror;
using UnityEngine;

namespace UI
{
    public class ConnectionToServerUI : MonoBehaviour
    {
        [SerializeField] private CustomNetworkManager customNetworkManager;
        [SerializeField] private TelepathyTransport kcpTransport;
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
            kcpTransport.port = Convert.ToUInt16(_port);
            customNetworkManager.StartClient();
        }

        public void AutoConnectionToServer()
        {
            customNetworkManager.networkAddress = "83.69.28.185";
            kcpTransport.port = 48877;
            customNetworkManager.StartClient();
        }
    }
}