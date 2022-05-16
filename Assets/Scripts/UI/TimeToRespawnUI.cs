using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimeToRespawnUI : MonoBehaviour
    {
        private float _timeLeftToRespawn;
        private Text _coutingTextUI;

        private void Start()
        {
            _coutingTextUI = GetComponent<Text>();
        }

        public void StartCounting(float timeActive)
        {
            _coutingTextUI.enabled = true;
            _timeLeftToRespawn = timeActive;
            StartCoroutine(Counting());
        }

        private IEnumerator Counting()
        {
            while (_timeLeftToRespawn > 0)
            {
                _timeLeftToRespawn -= Time.deltaTime;
                _coutingTextUI.text = "Возрождение через: " + Mathf.Round(_timeLeftToRespawn);
                yield return null;
            }
            _coutingTextUI.enabled = false;
        }
    }
}