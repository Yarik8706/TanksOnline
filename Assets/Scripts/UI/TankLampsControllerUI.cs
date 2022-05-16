using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TankLampsControllerUI : MonoBehaviour
    {
        public Text turretLampActionText;
        public Text bodyLampsActionText;

        private void Update()
        {
            turretLampActionText.text = Tank.localTank.turretLampOn ? "Выключить свет на пушке" : "Включить свет на пушке";
            bodyLampsActionText.text = Tank.localTank.bodyLampOn ? "Выключить свет на копусе" : "Включить свет на корпусе";
        }

        public void ChangeBodyLampsState()
        {
            Tank.localTank.ChangeBodyLampState();
        }

        public void ChangeTurretLampState()
        {
            Tank.localTank.ChangeTurretLampState();
        }
    }
}