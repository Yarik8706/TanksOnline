using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthTextUI : MonoBehaviour
    {
        private Text _textUI;

        private void Start()
        {
            _textUI = GetComponent<Text>();
        }

        private void Update()
        {
            if(Tank.localTank == null) return;
            _textUI.text = new string('-', Tank.localTank.health);
        }
    }
}