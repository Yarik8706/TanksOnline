using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ClientConnectionButtonUI : MonoBehaviour
    {
        public Text title;
        public string hostname;
        public string serverName;
        public LanManagerController connection;

        private void Start()
        {
            title.text = serverName;
        }

        public void ClientConnection()
        {
            connection.ConnectionToServer(hostname);
        }
    }
}