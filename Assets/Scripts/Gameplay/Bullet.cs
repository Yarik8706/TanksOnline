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
            base.OnStartServer();
            Invoke(nameof(DestroySelf), destroyAfter);
        }

        private void Start()
        {
            rigidBody.AddForce(transform.forward * force);
        }

        [ServerCallback]
        private void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }
        
        private void SpawnEffect()
        {
            var effect = Instantiate(deadEffect, transform.position + transform.forward * -.4f, Quaternion.identity);
            NetworkServer.Spawn(effect);
        }
        
        [ServerCallback]
        private void OnTriggerEnter(Collider _)
        {
            SpawnEffect();
            DestroySelf();
        }
    }
}