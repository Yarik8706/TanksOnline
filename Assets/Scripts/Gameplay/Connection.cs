using UnityEngine;

namespace Gameplay
{
    public class Connection : MonoBehaviour
    {
        private CustomNetworkManager _networkManager;

        private void Start()
        {
            _networkManager = GetComponent<CustomNetworkManager>();
        }

        public void StopAll()
        {
            _networkManager.StopClient();
            _networkManager.StopHost();
            _networkManager.StopServer();
        }
    
        public void StartHost()
        {
            _networkManager.StartHost();
        }

        public void StartClient(string ip)
        {
            _networkManager.networkAddress = ip;
            _networkManager.StartClient();
        }

        public void StartOnlyServer()
        {
            _networkManager.StartServer();
        }
    }
}