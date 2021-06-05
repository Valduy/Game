using System.Collections;
using Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menus
{
    public class AuthorizationStatusIndicator : MonoBehaviour
    {
        [SerializeField] private Image _indicator;
        [SerializeField] private TMP_Text _statusText;

        public Color UndefinedColor;
        public Color AuthorizedColor;
        public Color UnauthorizedColor;
        public Color NoConnectionColor;

        public void Refresh()
        {
            StartCoroutine(GetStatusCoroutine());
        }

        void OnEnable()
        {
            _indicator.color = UndefinedColor;
            ShowStatus("проверка авторизации");
            Refresh();
        }

        private IEnumerator GetStatusCoroutine()
        {
            var config = StreamingAssetsHelper.GetConfig();
            var url = $"{config.Url}/api/account/authorization";

            using (var request = UnityHttpHelper.Get(url, config.Token))
            {
                yield return request.SendWebRequest();

                switch (request.responseCode)
                {
                    case 200:
                        _indicator.color = AuthorizedColor;
                        ShowStatus("авторизован");
                        break;
                    case 401:
                        _indicator.color = UnauthorizedColor;
                        ShowStatus("неавторизован");
                        break;
                    default:
                        _indicator.color = NoConnectionColor;
                        ShowStatus("нет сети");
                        break;
                }
            }
        }

        private void ShowStatus(string status)
        {
            _statusText.gameObject.SetActive(false);
            _statusText.gameObject.GetComponent<TextMeshProUGUI>().text = status;
            _statusText.gameObject.SetActive(true);
        }
    }
}
