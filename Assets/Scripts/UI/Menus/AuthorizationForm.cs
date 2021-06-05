using System.Collections;
using Assets.Scripts.Models;
using Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menus
{
    public class AuthorizationForm : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _loginField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private TMP_Text _errorText;
        [SerializeField] private TMP_Text _successText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _authorizeButton;
        [SerializeField] private GameObject _loadCircle;

        public void Authorize()
        {
            DisableMessageTexts();
            var user = GetUserInformation();
            StartCoroutine(AuthorizationCoroutine(user));
        }

        void OnEnable()
        {
            DisableMessageTexts();
        }

        private void DisableMessageTexts()
        {
            _errorText.gameObject.SetActive(false);
            _successText.gameObject.SetActive(false);
        }

        private void SetInteractableUi(bool interactable)
        {
            _backButton.interactable = interactable;
            _authorizeButton.interactable = interactable;
            _loginField.interactable = interactable;
            _passwordField.interactable = interactable;
        }

        private User GetUserInformation() => new User
        {
            Login = _loginField.text,
            Password = _passwordField.text
        };

        private void ShowError(string error)
        {
            _errorText.gameObject.SetActive(false);
            _errorText.gameObject.GetComponent<TextMeshProUGUI>().text = error;
            _errorText.gameObject.SetActive(true);
        }

        private void ShowSuccess() => _successText.gameObject.SetActive(true);

        private IEnumerator AuthorizationCoroutine(User user)
        {
            SetInteractableUi(false);
            _loadCircle.SetActive(true);

            var config = StreamingAssetsHelper.GetConfig();
            var url = $"{config.Url}/api/account/authorization";
            var json = JsonUtility.ToJson(user);

            using (var request = UnityHttpHelper.Post(url, json))
            {
                yield return request.SendWebRequest();
                
                switch (request.responseCode)
                {
                    case 200:
                        ShowSuccess();
                        var session = JsonUtility.FromJson<Session>(request.downloadHandler.text);
                        config.Token = session.accessToken;
                        StreamingAssetsHelper.SaveConfig(config);
                        break;
                    case 401:
                        ShowError("Неверный логин или пароль.");
                        break;
                    default:
                        ShowError("Не удалось установить соединение с сервером.");
                        break;
                }
            }

            _loadCircle.SetActive(false);
            SetInteractableUi(true);
        }
    }
}
