using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class DeadController : MonoBehaviour
    {
        public void Active(GameObject deadObject, Vector3 nextPosition, float timeWait)
        {
            deadObject.transform.position = nextPosition;
            deadObject.SetActive(false);
            StartCoroutine(Respawn(deadObject, timeWait));
        }

        private IEnumerator Respawn(GameObject deadObject, float timeWait)
        {
            yield return new WaitForSeconds(timeWait);
            deadObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}