using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;

namespace Assets.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartSingle()
        {
            Debug.Log("Single!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void StartMultiplayer()
        {
            Debug.Log("Multiplayer!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }

        public void Authorize()
        {
            Debug.Log("Authorize");
        }

        public void Quit()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}
