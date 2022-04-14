using Mirror;

public class CustomNetworkManager : NetworkManager
{
    // public static CustomNetworkManager networkManager;
    // public static List<GameObject> players;

    // public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    // {
    //Transform startPos = GetStartPosition();
    //     GameObject player = startPos != null
    //         ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
    //         : Instantiate(playerPrefab);
    //     
    //     player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
    //     players.Add(player);
    //     NetworkServer.AddPlayerForConnection(conn, player);
    // }

    public override void OnStopServer()
    {
        KillCountsStatsUI.killCountsStatsUI.ClearAll();
        base.OnStopServer();
    }
}