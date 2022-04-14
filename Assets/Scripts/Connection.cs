using UnityEngine;

public class Connection : MonoBehaviour
{
    private CustomNetworkManager _networkManager;

    private void Start()
    {
        _networkManager = GetComponent<CustomNetworkManager>();
    }

    public void StopAll()
    {
        _networkManager.StopClient();
        _networkManager.StopHost();
        _networkManager.StopServer();
    }
    
    public void StartHost()
    {
        _networkManager.StartHost();
    }

    public void StartClient()
    {
        _networkManager.networkAddress = LoginUI.hostname;
        _networkManager.StartClient();
    }

    public void StartOnlyServer()
    {
        _networkManager.StartServer();
    }
}