using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthTextUI : MonoBehaviour
    {
        private Text textUI;

        private void Start()
        {
            textUI = GetComponent<Text>();
        }

        private void Update()
        {
            textUI.text = new string('-', Tank.localTank.health);
        }
    }
}