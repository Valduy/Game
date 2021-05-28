using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Models;
using Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menus
{
    public class RegistrationForm : MonoBehaviour
    {
        public TMP_InputField LoginField;
        public TMP_InputField PasswordField;
        public TMP_Text ErrorText;
        public TMP_Text SuccessText;
        public Button BackButton;
        public Button RegisterButton;
        public GameObject LoadCircle;

        public void Register()
        {
            DisableMessageTexts();
            var user = GetUserInformation();
            var errors = Validate(user);

            if (!errors.Any())
            {
                StartCoroutine(RegistrationCoroutine(user));
            }
            else
            {
                ShowError(errors.First());
            }
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

        private void SetInteractableUI(bool enable)
        {
            BackButton.interactable = enable;
            RegisterButton.interactable = enable;
            LoginField.interactable = enable;
            PasswordField.interactable = enable;
        }

        private User GetUserInformation() => new User
        {
            Login = LoginField.text,
            Password = PasswordField.text
        };

        private List<string> Validate(User user)
        {
            var errors = new List<string>();
            var regex = new Regex(@"^[a-zA-Z0-9_]+$");

            ValidatePasswordLength(user, errors);

            if (!regex.IsMatch(user.Login))
            {
                errors.Add("Логин содержит некорректные символы.");
            }
            
            ValidateLoginLength(user, errors);

            if (!regex.IsMatch(user.Password))
            {
                errors.Add("Пароль содержит некорректные символы.");
            }

            return errors;
        }

        private void ValidatePasswordLength(User user, List<string> errors)
        {
            if (user.Login.Length == 0)
            {
                errors.Add("Не указан логин.");
            }
            else if (user.Login.Length < 4)
            {
                errors.Add("Слишком короткий логин.");
            }
            else if (user.Login.Length > 20)
            {
                errors.Add("Слишком длинный логин.");
            }
        }

        private void ValidateLoginLength(User user, List<string> errors)
        {
            if (user.Password.Length == 0)
            {
                errors.Add("Не указан пароль.");
            }
            else if (user.Password.Length < 8)
            {
                errors.Add("Слишком короткий пароль.");
            }
            else if (user.Password.Length > 20)
            {
                errors.Add("Слишком длинный пароль.");
            }
        }

        private void ShowError(string error)
        {
            ErrorText.gameObject.SetActive(false);
            ErrorText.gameObject.GetComponent<TextMeshProUGUI>().text = error;
            ErrorText.gameObject.SetActive(true);
        }

        private void ShowSuccess() => SuccessText.gameObject.SetActive(true);

        private IEnumerator RegistrationCoroutine(User user)
        {
            SetInteractableUI(false);
            LoadCircle.SetActive(true);

            var config = StreamingAssetsHelper.GetConfig();
            var url = $"{config.Url}/api/account/registration";
            var json = JsonUtility.ToJson(user);
            var request = UnityHttpHelper.Post(url, json);

            yield return request.SendWebRequest();

            LoadCircle.SetActive(false);

            switch (request.responseCode)
            {
                case 200:
                    ShowSuccess();
                    break;
                case 409:
                    ShowError("Логин уже занят.");
                    break;
                default:
                    ShowError("Не удалось установить соединение с сервером.");
                    break;
            }

            SetInteractableUI(true);
        }
    }
}
