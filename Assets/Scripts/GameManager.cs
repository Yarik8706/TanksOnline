using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
    public Transform[] startPositions;
    public GameObject playerPrefab;

    public static GameObject respawnTime;
    public static GameManager gameManager;

    private void Start()
    {
        gameManager = this;
    }

    [Server]
    public IEnumerator IsDead(Tank delObject)
    {
        NetworkServer.Destroy(delObject.gameObject);
        yield return new WaitForSeconds(8f);
        var player = Instantiate(playerPrefab,
            startPositions[Random.Range(0, startPositions.Length)].position,
            Quaternion.identity);
    }
}