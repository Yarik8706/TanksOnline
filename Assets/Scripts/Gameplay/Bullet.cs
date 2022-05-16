using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class Bullet : NetworkBehaviour
    {
        [Header("Stats")]
        public float destroyAfter = 2;
        public float force = 1000;
    
        [Header("Components")]
        public Rigidbody rigidBody;
        public GameObject deadEffect;
    
        internal GameObject creater;

        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), destroyAfter);
        }

        private void Start()
        {
            rigidBody.AddForce(transform.forward * force);
        }

        [ServerCallback]
        private void DestroySelf()
        {
            if(gameObject != null) NetworkServer.Destroy(gameObject);
        }
        
        private void SpawnEffect()
        {
            Instantiate(deadEffect, transform.position, Quaternion.identity);
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            SpawnEffect();
            DestroySelf();
        }
    }
}