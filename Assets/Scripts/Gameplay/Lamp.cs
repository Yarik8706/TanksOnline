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
        private float _blinkTimeNow;

        public override void OnStartClient()
        {
            base.OnStartClient();
            _lampsControllerUI = FindObjectOfType<LampsControllerUI>();
        }

        private void Start()
        {
            _blinkTimeNow = blinkTime;
            _light = GetComponent<Light>();
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
                    if (_blinkTimeNow <= 0)
                    {
                        _light.enabled = !_light.enabled;
                        _blinkTimeNow = blinkTime;
                        return;
                    }
                    _blinkTimeNow -= Time.deltaTime;
                    break;
            }
        }

        [Command]
        public void ChangeLampActive()
        {
            lampState = lampState == (int)LampState.On ? (int)LampState.Off : (int)LampState.On;
        }

        [Command]
        public void ChangeLampBlickActive()
        {
            lampState = lampState == (int)LampState.On ? (int)LampState.Blink : (int)LampState.On;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Tank>() is {isLocalPlayer: true})
            {
                if(_lampsControllerUI == null) GetLampsController();
                _lampsControllerUI.ActiveButtons(this);
            }
        }

        private void GetLampsController()
        {
            _lampsControllerUI = FindObjectOfType<LampsControllerUI>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<Tank>() is {isLocalPlayer: true})
            {
                if(_lampsControllerUI == null) GetLampsController();
                _lampsControllerUI.ActiveButtons(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(_lampsControllerUI == null) GetLampsController();
            _lampsControllerUI.EnableButtons();
        }
    }
}
