using System.Collections;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public Transform[] startPositions;
        public static GameManager gameManager;

        private void Start()
        {
            
            gameManager = this;
        }
    }
}