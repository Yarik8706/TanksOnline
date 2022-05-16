using Mirror;
using UI;

namespace Gameplay
{
    public class CustomNetworkManager : NetworkManager
    {
        public KillCountsStatsUI killCountsStatsUI;
        
        public override void OnClientDisconnect()
        {
            killCountsStatsUI.ClearAll();
            Tank.localTank.SwitchCameras(true);
            base.OnClientDisconnect();
        }

        public override void OnStopServer()
        {
            killCountsStatsUI.ClearAll();
            Tank.localTank.SwitchCameras(true);
            base.OnStopServer();
        }
    }
}