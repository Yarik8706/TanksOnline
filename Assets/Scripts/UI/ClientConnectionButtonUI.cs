using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ClientConnectionButtonUI : MonoBehaviour
    {
        public Text title;
        public string hostname;
        public string serverName;
        public ConnectionController connection;

        private void Start()
        {
            title.text = serverName;
        }

        public void ClientConnection()
        {
            connection.StartClient(hostname);
        }
    }
}