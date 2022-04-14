using UnityEngine;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    public GameObject connectionUI;
    public Text text;

    private bool _active;
    
    public void ChangeActive()
    {
        _active = !_active;
        connectionUI.SetActive(_active);
        text.text = _active ? "Закрыть" : "Подключение";
    }
}