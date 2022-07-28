using UnityEngine;

namespace UI
{
    public class StateConnectionButtonUI : MonoBehaviour
    {
        public GameObject gameplayUI;
        public GameObject connectionUI;
        public GameObject findServersUI;
        
        public void ChangeConnectionUIState()
        {
            connectionUI.SetActive(gameplayUI.activeSelf);
            gameplayUI.SetActive(!gameplayUI.activeSelf);
            findServersUI.SetActive(false);
        }

        public void SetConnectionUIState(bool openGameplayUI)
        {
            connectionUI.SetActive(!openGameplayUI);
            gameplayUI.SetActive(openGameplayUI);
            findServersUI.SetActive(false);
        }
    }
}