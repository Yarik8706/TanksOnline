using System.Collections;
using Mirror;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestroyEffect : NetworkBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(nameof(CheckIfAlive));
        }

        private IEnumerator CheckIfAlive ()
        {
            var ps = GetComponent<ParticleSystem>();
            while(ps != null)
            {
                yield return new WaitForSeconds(0.5f);
                if (ps.IsAlive(true)) continue;
                NetworkServer.Destroy(gameObject);
                break;
            }
        }
    }
}