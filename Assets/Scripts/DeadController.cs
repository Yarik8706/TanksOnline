using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeadController : MonoBehaviour
{
    public void Active(GameObject deadObject)
    {
        deadObject.SetActive(false);
        StartCoroutine(Respawn(deadObject));
    }

    private IEnumerator Respawn(GameObject deadObject)
    {
        yield return new WaitForSeconds(5);
        deadObject.SetActive(true);
        deadObject.transform.position =
            GameManager.gameManager.startPositions[Random.Range(0, GameManager.gameManager.startPositions.Length)].position;
        Destroy(gameObject);
    }
}