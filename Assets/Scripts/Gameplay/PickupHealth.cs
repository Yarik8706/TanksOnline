using Mirror;
using UnityEngine;

namespace Gameplay
{
    public class PickupHealth : NetworkBehaviour
    {
        public int amountHealth;

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            var tank = other.GetComponent<Tank>();
            if(tank == null) return;
            tank.health += amountHealth;
            NetworkServer.Destroy(gameObject);
        }
    }
}
