using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AttackButtonUI : MonoBehaviour
    {
        public Text timeAttack;

        private void Start()
        {
            Tank.speedAttackTimeText = timeAttack;
        }

        public void OnClick()
        {
            Tank.localTank.Attack();
        }
    }
}