using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    public FixedJoystick joystickForward;
    public FixedJoystick joystickTurretRotation;
    public GameObject respawnTime;
    public KillCountsStatsUI killCountStatsUI;

    private void Start()
    {
        InputHandler.joystickTurretRotation = joystickTurretRotation;
        InputHandler.joystickMovement = joystickForward;
        GameManager.respawnTime = respawnTime;
        KillCountsStatsUI.killCountsStatsUI = killCountStatsUI;
    }
}