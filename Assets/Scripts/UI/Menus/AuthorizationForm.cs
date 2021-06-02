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
        public TMP_InputField LoginField;
        public TMP_InputField PasswordField;
        public TMP_Text ErrorText;
        public TMP_Text SuccessText;
        public Button BackButton;
        public Button AuthorizeButton;
        public GameObject LoadCircle;

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
            ErrorText.gameObject.SetActive(false);
            SuccessText.gameObject.SetActive(false);
        }

        private void SetInteractableUi(bool interactable)
        {
            BackButton.interactable = interactable;
            AuthorizeButton.interactable = interactable;
            LoginField.interactable = interactable;
            PasswordField.interactable = interactable;
        }

        private User GetUserInformation() => new User
        {
            Login = LoginField.text,
            Password = PasswordField.text
        };

        private void ShowError(string error)
        {
            ErrorText.gameObject.SetActive(false);
            ErrorText.gameObject.GetComponent<TextMeshProUGUI>().text = error;
            ErrorText.gameObject.SetActive(true);
        }

        private void ShowSuccess() => SuccessText.gameObject.SetActive(true);

        private IEnumerator AuthorizationCoroutine(User user)
        {
            SetInteractableUi(false);
            LoadCircle.SetActive(true);

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

            LoadCircle.SetActive(false);
            SetInteractableUi(true);
        }
    }
}
