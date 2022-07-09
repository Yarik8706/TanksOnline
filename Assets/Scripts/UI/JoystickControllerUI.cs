using Gameplay;
using UnityEngine;

namespace UI
{
    public class JoystickControllerUI : MonoBehaviour
    {
        public FixedJoystick joystickForward;
        public FixedJoystick joystickTurretRotation;

        private void Start()
        {
            InputHandler.joystickTurretRotation = joystickTurretRotation;
            InputHandler.joystickMovement = joystickForward;
        }
    }
}