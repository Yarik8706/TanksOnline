using System.Collections.Generic;
using System.Net;
using Gameplay;
using Mirror.Discovery;
using UI;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{
    private CustomNetworkManager _networkManager;
    private NetworkDiscovery _networkDiscovery;
    private List<IPAddress> _foundServerAddresses;
    public GameObject connectionCanvas;
    public GameObject gameplayCanvas;
    public GameObject findServersCanvas;
    public GameObject serversContainer;
    public GameObject availabaleServerButton;

    private void Start()
    {
        //NetworkDiscoveryHUD
        _networkDiscovery = FindObjectOfType<NetworkDiscovery>();
        _networkDiscovery.OnServerFound.AddListener(AddServer);
        _networkManager = FindObjectOfType<CustomNetworkManager>();
    }

    public void StopAll()
    {
        _networkManager.StopClient();
        _networkManager.StopHost();
        _networkManager.StopServer();
    }
    
    public void StartHost()
    {
        connectionCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
        _networkManager.StartHost();
        _networkDiscovery.AdvertiseServer();
    }

    public void StartClient(string ip)
    {
        gameplayCanvas.SetActive(true);
        findServersCanvas.SetActive(false);
        connectionCanvas.SetActive(false);
        _networkManager.networkAddress = ip;
        _networkManager.StartClient();
    }

    public void StartOnlyServer()
    {
        gameplayCanvas.SetActive(true);
        findServersCanvas.SetActive(false);
        connectionCanvas.SetActive(false);
        _networkManager.StartServer();
    }

    public void FindServers()
    {
        _foundServerAddresses = new List<IPAddress>();
        gameplayCanvas.SetActive(false);
        findServersCanvas.SetActive(true);
        connectionCanvas.SetActive(false);
        _networkDiscovery.StartDiscovery();
    }

    public void StopFindServers()
    {
        gameplayCanvas.SetActive(false);
        findServersCanvas.SetActive(false);
        connectionCanvas.SetActive(true);
        _networkDiscovery.StopDiscovery();
    }

    public void AddServer(ServerResponse data)
    {
        if(_foundServerAddresses.Contains(data.EndPoint.Address) || string.IsNullOrEmpty(data.EndPoint.Address.ToString())) return;
        var connectionButton = Instantiate(availabaleServerButton, serversContainer.transform, true)
            .GetComponent<ClientConnectionButtonUI>();
        _foundServerAddresses.Add(data.EndPoint.Address);
        connectionButton.connection = this;
        connectionButton.hostname = data.EndPoint.Address.ToString();
        connectionButton.serverName = data.EndPoint.Address.ToString();
    }

    public void ChangeConnectionMenuState()
    {
        gameplayCanvas.SetActive(!gameplayCanvas.activeSelf);
        findServersCanvas.SetActive(false);
        connectionCanvas.SetActive(!connectionCanvas.activeSelf);
    }
}