using System.Collections;
using Gameplay;
using UI;
using UnityEngine;

public class LanManagerController : MonoBehaviour
{
    public GameObject textSearchingPlayer;
    public GameObject textNotActivePlayer;
    public GameObject activeServersContainer;
    public GameObject buttonForConnectionPrefab;
    public Connection connection;
    
    private LanManager _lanManager;
    public const int Port = 4500;
 
    private void Start()
    {
        _lanManager = GetComponent<LanManager>();
    }

    public void StartConnectionClient()
    {
        _lanManager.StartClient(Port);
        _lanManager.ScanHost();
        FindServers();
    }

    public void StartConnectionServer()
    {
        _lanManager.StartServer(Port);
        _lanManager.ScanHost();
    }

    public void FindServers()
    {
        ClearActiveServer();
        StartCoroutine(_lanManager.SendPing(Port));
        StartCoroutine(WaitFindServers());
    }

    private IEnumerator WaitFindServers()
    {
        textNotActivePlayer.SetActive(false);
        textSearchingPlayer.SetActive(true);
        yield return new WaitUntil(() => !_lanManager.IsSearching);
        textSearchingPlayer.SetActive(false);
        UpdateActiveServer();
        if (_lanManager.Addresses.Count == 0)
        {
            textNotActivePlayer.SetActive(true);
        }
    }

    public void StopAll()
    {
        _lanManager.CloseClient();
        _lanManager.CloseServer();
    }

    public void ConnectionToServer(string ip)
    {
        connection.StartClient(ip);
        ClearActiveServer();
        StopAll();
        GetComponent<ButtonsHandlerUI>().loginActiveButtonUI.CloseLoginUI();
    }

    private void UpdateActiveServer()
    {
        for (var i = 0; i < _lanManager.Addresses.Count; i++)
        {
            var button = Instantiate(buttonForConnectionPrefab, activeServersContainer.transform, true).GetComponent<ClientConnectionButtonUI>();
            button.hostname = _lanManager.Addresses[i];
            button.serverName = _lanManager.ServerNames[i];
            button.connection = this;
        }
    }

    private void ClearActiveServer()
    {
        for (int i = 0; i < activeServersContainer.transform.childCount; i++)
        {
            Destroy(activeServersContainer.transform.GetChild(i).gameObject);
        }
    }
}
