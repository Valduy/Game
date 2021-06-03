using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Util;
using Connectors.HolePuncher;
using Connectors.MatchConnectors;
using Connectors.MatchmakerConnectors;
using Network;
using Network.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Loading
{
    public class SearchMatch : MonoBehaviour
    {
        private CancellationTokenSource _tokenSource;

        [SerializeField] private Button _cancelButton;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private GameObject _errorPanel;

        public void Cancel()
        {
            _tokenSource.Cancel();
        }

        async void Start()
        {
            try
            {
                _tokenSource = new CancellationTokenSource();
                await SearchMatchAsync(_tokenSource.Token);
                Debug.Log("Все хорошо.");
                // TODO: загрузка сцены, в зависимости от роли
            }
            catch (OperationCanceledException)
            {
                MatchData.UdpClient?.Dispose();
                SceneManager.LoadScene("MainMenu");
            }
            catch (HttpRequestException)
            {
                _errorPanel.SetActive(true);
                ShowError("Произошла ошибка при попытке подключиться к серверу.");
                _cancelButton.interactable = false;
            }
            catch (Exception e)
            {
                _errorPanel.SetActive(true);
                ShowError(e.Message);
                _cancelButton.interactable = false;
            }
        }

        void OnDestroy()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }
        private async Task SearchMatchAsync(CancellationToken cancellationToken)
        {
            MatchData.UdpClient = new UdpClient(0);
            var config = StreamingAssetsHelper.GetConfig();
            var privateEndPoint = new ClientEndPoint(NetworkHelper.GetLocalIPAddress(), MatchData.UdpClient.GetPort());

            ShowStatus("Поиск матча...");
            var matchmakerConnector = new MatchmakerConnector();
            var port = await matchmakerConnector.ConnectAsync(privateEndPoint, config.Url, config.Token, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            ShowStatus("Получение данных...");
            var matchConnector = new MatchConnector();
            var message = await matchConnector.ConnectAsync(MatchData.UdpClient, config.Ip, port, cancellationToken);
            MatchData.Role = message.Role;
            cancellationToken.ThrowIfCancellationRequested();

            ShowStatus("Соединение...");
            var holePuncher = new HolePuncher();
            MatchData.Clients = await holePuncher.ConnectAsync(MatchData.UdpClient, message.Clients, cancellationToken);
        }

        private void ShowStatus(string status)
        {
            _statusText.gameObject.SetActive(false);
            _statusText.GetComponent<TextMeshProUGUI>().text = status;
            _statusText.gameObject.SetActive(true);
        }

        private void ShowError(string errorMessage) 
            => _errorPanel.GetComponent<ErrorPanel>().ErrorMessage = errorMessage;
    }
}
