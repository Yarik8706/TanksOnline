using System;
using Mirror;
using UnityEngine;

namespace UI
{
    public class OnServerBuild : MonoBehaviour
    {
        private void Awake()
        {
            var mirror = GetComponent<NetworkManager>();
            mirror.StartServer();
        }
    }
}