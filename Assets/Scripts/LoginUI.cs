using UnityEngine;

public class LoginUI : MonoBehaviour
{
    public static string namePlayer;
    public static string hostname;

    public void SetPlayerName(string value)
    {
        namePlayer = value;
    }

    public void SetHostname(string value)
    {
        hostname = value;
    }
}