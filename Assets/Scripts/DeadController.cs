using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeadController : MonoBehaviour
{
    public void Active(GameObject deadObject, Vector3 nextPosition)
    {
        deadObject.transform.position = nextPosition;
        deadObject.SetActive(false);
        StartCoroutine(Respawn(deadObject));
    }

    private IEnumerator Respawn(GameObject deadObject)
    {
        yield return new WaitForSeconds(5);
        deadObject.SetActive(true);
        Destroy(gameObject);
    }
}