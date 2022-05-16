using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoginActiveButtonUI : MonoBehaviour
    {
        public GameObject canvas;
        public GameObject loginUI;
        public Text openCloseText;

        public void ChangeActiveLoginUI()
        {
            SetActiveLoginUI(!loginUI.activeSelf);
        }

        public void CloseLoginUI()
        {
            openCloseText.text = "Подключение";
            SetActiveLoginUI(false);
        }

        private void SetActiveLoginUI(bool value)
        {
            canvas.SetActive(!value);
            loginUI.SetActive(value);
            openCloseText.text = value ? "Закрыть" : "Подключение";
        }
    }
}