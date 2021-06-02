using Assets.Scripts.UI.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;

namespace Assets.Scripts.UI.Menus
{
    public class MainMenu : MonoBehaviour
    {
        public void StartSingle()
        {
            LoadingData.NextScene = "Single";
            SceneManager.LoadScene("Loading");
        }

        public void StartMultiplayer()
        {
            SceneManager.LoadScene("SearchMatch");
        }

        public void Quit()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}
