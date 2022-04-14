using Mirror;
using UnityEngine;

public class InputHandler : NetworkBehaviour
{
    public static FixedJoystick joystickMovement;
    public static FixedJoystick joystickTurretRotation;

    public static Vector3 GetTurretRotation(Vector3 position)
    {
        return new Vector3(position.x - joystickTurretRotation.Horizontal, 
            position.y, 
            position.z - joystickTurretRotation.Vertical);
    }

    public static float GetHorizontalMovementChanges()
    {
        return joystickMovement.Horizontal;
    }

    public static float GetVerticalMovementChanges()
    {
        return joystickMovement.Vertical;
    }

    private static bool CanProcessInput()
    {
        return true;
    }
}