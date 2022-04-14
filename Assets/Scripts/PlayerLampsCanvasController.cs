using UnityEngine;

public class PlayerLampsCanvasController : MonoBehaviour
{
    public void ChangeBodyLampsState()
    {
        Tank.localPlayerTank.ChangeBodyLampState();
    }

    public void ChangeTurretLampState()
    {
        Tank.localPlayerTank.ChangeTurretLampState();
    }
}