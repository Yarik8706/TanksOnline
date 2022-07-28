using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ReloadSceneButtonUI : MonoBehaviour
    {
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}