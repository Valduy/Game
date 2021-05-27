using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        public Image ProgressBar;

        public void Start()
        {
            StartCoroutine(LoadSceneCoroutine());
        }

        private IEnumerator LoadSceneCoroutine()
        {
            var sceneToLoad = SceneManager.LoadSceneAsync(LoadingData.NextScene);

            while (sceneToLoad.isDone)
            {
                ProgressBar.fillAmount = sceneToLoad.progress;
                yield return new WaitForEndOfFrame();
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
}
