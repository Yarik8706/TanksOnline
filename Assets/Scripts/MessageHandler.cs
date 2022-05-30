using UnityEngine;
using UnityEngine.UI;

public class MessageHandler : MonoBehaviour
{
    public GameObject messageContainer;
    public GameObject textPrefab;

    public void AddMessage(string message)
    {
        Instantiate(textPrefab, messageContainer.transform, true).GetComponent<Text>().text = message;
    }
}