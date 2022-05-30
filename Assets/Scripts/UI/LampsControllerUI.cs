using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LampsControllerUI : MonoBehaviour
    {
        [Header("Components")] 
        public Text lampActiveButton;
        public Text makeLampBlinkButton;
        public GameObject containerWithButtons;

        private Lamp _activeLamp;
        
        private void Update()
        {
            if(_activeLamp == null) return;
            switch (_activeLamp.lampState)
            {
                case (int)LampState.On:
                    lampActiveButton.text = "Выключить лампу";
                    makeLampBlinkButton.transform.parent.gameObject.SetActive(true);
                    makeLampBlinkButton.text = "Заставить лампу мигать";
                    break;
                case (int)LampState.Off:
                    lampActiveButton.text = "Включить лампу";
                    makeLampBlinkButton.transform.parent.gameObject.SetActive(false);
                    break;
                case (int)LampState.Blink:
                    makeLampBlinkButton.transform.parent.gameObject.SetActive(true);
                    lampActiveButton.text = "Выключить лампу";
                    makeLampBlinkButton.text = "Прекратить мигать лампу";
                    break;
            }
        }

        public void ActiveButtons(Lamp activeLamp)
        {
            _activeLamp = activeLamp;
            containerWithButtons.SetActive(true);
        }

        public void EnableButtons()
        {
            _activeLamp = null;
            containerWithButtons.SetActive(false);
        }

        public void ChangeLampActive()
        {
            _activeLamp.ChangeLampActive();
        }

        public void MakeLampBlink()
        {
            _activeLamp.ChangeLampBlickActive();
        }
    }
}
