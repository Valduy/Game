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
        [SerializeField] private TMP_InputField _loginField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private TMP_Text _errorText;
        [SerializeField] private TMP_Text _successText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _registerButton;
        [SerializeField] private GameObject _loadCircle;

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
            _errorText.gameObject.SetActive(false);
            _successText.gameObject.SetActive(false);
        }

        private void SetInteractableUi(bool interactable)
        {
            _backButton.interactable = interactable;
            _registerButton.interactable = interactable;
            _loginField.interactable = interactable;
            _passwordField.interactable = interactable;
        }

        private User GetUserInformation() => new User
        {
            Login = _loginField.text,
            Password = _passwordField.text
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
            _errorText.gameObject.SetActive(false);
            _errorText.gameObject.GetComponent<TextMeshProUGUI>().text = error;
            _errorText.gameObject.SetActive(true);
        }

        private void ShowSuccess() => _successText.gameObject.SetActive(true);

        private IEnumerator RegistrationCoroutine(User user)
        {
            SetInteractableUi(false);
            _loadCircle.SetActive(true);

            var config = StreamingAssetsHelper.GetConfig();
            var url = $"{config.Url}/api/account/registration";
            var json = JsonUtility.ToJson(user);

            using (var request = UnityHttpHelper.Post(url, json))
            {
                yield return request.SendWebRequest();

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
            }

            _loadCircle.SetActive(false);
            SetInteractableUi(true);
        }
    }
}
