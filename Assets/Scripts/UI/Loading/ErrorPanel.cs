using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Loading
{
    public class ErrorPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _errorMessageText;

        public string ErrorMessage
        {
            get => _errorMessageText.GetComponent<TextMeshProUGUI>().text;
            set => _errorMessageText.GetComponent<TextMeshProUGUI>().text = value;
        }

        public void Ok()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
