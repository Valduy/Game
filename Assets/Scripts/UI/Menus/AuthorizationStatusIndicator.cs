using System.Collections;
using Assets.Scripts.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menus
{
    public class AuthorizationStatusIndicator : MonoBehaviour
    {
        public Color UndefinedColor;
        public Color AuthorizedColor;
        public Color UnauthorizedColor;
        public Color NoConnectionColor;
        public Image Indicator;
        public TMP_Text StatusText;

        public void Refresh()
        {
            StartCoroutine(GetStatusCoroutine());
        }

        void OnEnable()
        {
            Indicator.color = UndefinedColor;
            ShowStatus("проверка авторизации");
            Refresh();
        }

        private IEnumerator GetStatusCoroutine()
        {
            var config = StreamingAssetsHelper.GetConfig();
            var url = $"{config.Url}/api/account/authorization";
            var request = UnityHttpHelper.Get(url, config.Token);
            
            yield return request.SendWebRequest();

            switch (request.responseCode)
            {
                case 200:
                    Indicator.color = AuthorizedColor;
                    ShowStatus("авторизован");
                    break;
                case 401:
                    Indicator.color = UnauthorizedColor;
                    ShowStatus("неавторизован");
                    break;
                default:
                    Indicator.color = NoConnectionColor;
                    ShowStatus("нет сети");
                    break;
            }
        }

        private void ShowStatus(string status)
        {
            StatusText.gameObject.SetActive(false);
            StatusText.gameObject.GetComponent<TextMeshProUGUI>().text = status;
            StatusText.gameObject.SetActive(true);
        }
    }
}
