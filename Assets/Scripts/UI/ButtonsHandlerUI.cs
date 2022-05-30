using Gameplay;
using UnityEngine;

namespace UI
{
    public class ButtonsHandlerUI : MonoBehaviour
    {
        public GameObject loginUI;
        public GameObject findPlayersUI;
        public Connection connection;
        public LoginActiveButtonUI loginActiveButtonUI;
        
        private LanManagerController _lanManagerController;

        private void Start()
        {
            _lanManagerController = GetComponent<LanManagerController>();
        }

        public void StartHost()
        {
            StopAll();
            connection.StartHost();
            _lanManagerController.StartConnectionServer();
            loginActiveButtonUI.CloseLoginUI();
        }

        public void StartClient()
        {
            StopAll();
            _lanManagerController.StartConnectionClient();
            ChangeUILayout();
        }

        public void StartOnlyServer()
        {
            StopAll();
            connection.StartOnlyServer();
            _lanManagerController.StartConnectionServer();
            loginActiveButtonUI.CloseLoginUI();
        }

        public void StopAll()
        {
            connection.StopAll();
            _lanManagerController.StopAll();
        }

        public void ExitFindPlayersUI()
        {
            StopAll();
            ChangeUILayout();
        }

        public void ChangeUILayout()
        {
            loginUI.SetActive(!loginUI.activeSelf);
            findPlayersUI.SetActive(!findPlayersUI.activeSelf);
        }
    }
}