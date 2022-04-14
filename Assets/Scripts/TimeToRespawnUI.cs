using UnityEngine;
using UnityEngine.UI;

public class TimeToRespawnUI : MonoBehaviour
{
    public static TimeToRespawnUI timeToRespawnUI;
    
    internal bool isDead;
    
    private float _timeLeftToRespawn;
    private Text _text;

    private void Start()
    {
        timeToRespawnUI = this;
        _text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        _timeLeftToRespawn = 5;
        isDead = true;
    }

    private void Update()
    {
        if (_timeLeftToRespawn > 0 && isDead)
        {
            _timeLeftToRespawn -= Time.deltaTime;
            _text.text = "Возрождение через: " + Mathf.Round(_timeLeftToRespawn);
        }
        else if(isDead)
        {
            gameObject.SetActive(false);
        }
    }
}