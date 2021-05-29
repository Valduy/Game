using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menus
{
    public enum AuthorizationStatus
    {
        Undefined,
        Authorized,
        Unauthorized
    }

    public class AuthorizationStatusIndicator : MonoBehaviour
    {
        private AuthorizationStatus _authorizationStatus;

        public Image GrayCircle;
        public Image GreenCircle;
        public Image RedCircle;
        public TMP_Text StatusText;

        public AuthorizationStatus AuthorizationStatus
        {
            get => _authorizationStatus;
            set
            {
                _authorizationStatus = value;

                switch (_authorizationStatus)
                {
                    case AuthorizationStatus.Undefined:
                        GrayCircle.enabled = true;
                        GreenCircle.enabled = false;
                        RedCircle.enabled = false;
                        ShowStatus("проверка авторизации");
                        break;
                    case AuthorizationStatus.Authorized:
                        GrayCircle.enabled = false;
                        GreenCircle.enabled = true;
                        RedCircle.enabled = false;
                        ShowStatus("авторизован");
                        break;
                    case AuthorizationStatus.Unauthorized:
                        GrayCircle.enabled = false;
                        GreenCircle.enabled = false;
                        RedCircle.enabled = true;
                        ShowStatus("неавторизации");
                        break;
                }
            }
        }

        void Start()
        {
            AuthorizationStatus = AuthorizationStatus.Undefined;
        }

        private void ShowStatus(string status)
        {
            StatusText.gameObject.SetActive(false);
            StatusText.gameObject.GetComponent<TextMeshProUGUI>().text = status;
            StatusText.gameObject.SetActive(true);
        }
    }
}
