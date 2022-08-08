using System;
using Mirror;
using UI;

namespace Gameplay
{
    public class CustomNetworkManager : NetworkManager
    {
        public KillCountsStatsUI killCountsStatsUI;
        public ConnectionController stateConnectionButtonUI;

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            stateConnectionButtonUI.ChangeConnectionMenuState(true);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            stateConnectionButtonUI.ChangeConnectionMenuState(true);
        }

        public override void OnClientError(Exception exception)
        {
            base.OnClientError(exception);
            // Tank.localTank.killCountStats.DeletePlayerStats(LoginUI.playerName);
            // killCountsStatsUI.ClearAll();
            Tank.localTank.SwitchCameras(true);
        }

        public override void OnServerError(NetworkConnectionToClient conn, Exception exception)
        {
            base.OnServerError(conn, exception);
            // Tank.localTank.killCountStats.DeletePlayerStats(LoginUI.playerName);
            // killCountsStatsUI.ClearAll();
            if(Tank.localTank != null) Tank.localTank.SwitchCameras(true);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            killCountsStatsUI.ClearAll();
            if(Tank.localTank != null) Tank.localTank.SwitchCameras(true);
        }

        public override void OnStopServer()
        {
            // Tank.localTank.killCountStats.DeletePlayerStats(LoginUI.playerName);
            killCountsStatsUI.ClearAll();
            if(Tank.localTank != null) Tank.localTank.SwitchCameras(true);
            base.OnStopServer();
        }
    }
}