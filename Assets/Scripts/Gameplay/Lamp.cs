using System;
using Mirror;
using UI;
using UnityEngine;

namespace Gameplay
{
    public enum LampState
    {
        Off, 
        On, 
        Blink
    }
    
    public class Lamp : NetworkBehaviour
    {
        [SyncVar] public int lampState;
        private Light _light;
        private LampsControllerUI _lampsControllerUI;
        public float blinkTime = 1f;
        private float blinkTimeNow;

        private void Start()
        {
            blinkTimeNow = blinkTime;
            _light = GetComponent<Light>();
            _lampsControllerUI = FindObjectOfType<LampsControllerUI>();
        }

        private void Update()
        {
            switch (lampState)
            {
                case (int)LampState.On:
                    _light.enabled = true;
                    break;
                case (int)LampState.Off:
                    _light.enabled = false;
                    break;
                case (int)LampState.Blink:
                    if (blinkTimeNow <= 0)
                    {
                        _light.enabled = !_light.enabled;
                        blinkTimeNow = blinkTime;
                        return;
                    }
                    blinkTimeNow -= Time.deltaTime;
                    break;
            }
        }

        public void ChangeLampActive()
        {
            lampState = lampState == (int)LampState.On ? (int)LampState.Off : (int)LampState.On;
        }

        public void ChangeLampBlickActive()
        {
            lampState = lampState == (int)LampState.On ? (int)LampState.Blink : (int)LampState.On;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Tank>() is {isLocalPlayer: true})
            {
                _lampsControllerUI.ActiveButtons(this);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<Tank>() is {isLocalPlayer: true})
            {
                _lampsControllerUI.ActiveButtons(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _lampsControllerUI.EnableButtons();
        }
    }
}
