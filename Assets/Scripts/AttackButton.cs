using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public Text timeAttack;

    private void Start()
    {
        Tank.speedAttackTimeText = timeAttack;
    }

    public void OnClick()
    {
        Tank.localPlayerTank.Attack();
    }
}