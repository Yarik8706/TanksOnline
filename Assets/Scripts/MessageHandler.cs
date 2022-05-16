using UnityEngine;
using UnityEngine.UI;

public class MessageHandler : MonoBehaviour
{
    public GameObject messageContainer;
    public GameObject textPrefab;

    public void AddMessage(string message)
    {
        // var newMessage = Instantiate(textPrefab, messageContainer.transform, true);
        // newMessage.GetComponent<Text>().text = message;
    }
}