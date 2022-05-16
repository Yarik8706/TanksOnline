using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class InputHandler : NetworkBehaviour
    {
        public static FixedJoystick joystickMovement;
        public static FixedJoystick joystickTurretRotation;

        public static Vector3 GetTurretRotation()
        {
            return new Vector3(joystickTurretRotation.Horizontal, 
                0, 
                joystickTurretRotation.Vertical);
        }

        public static float GetHorizontalMovementChanges()
        {
            return joystickMovement.Horizontal;
        }

        public static float GetVerticalMovementChanges()
        {
            return joystickMovement.Vertical;
        }
    }
}