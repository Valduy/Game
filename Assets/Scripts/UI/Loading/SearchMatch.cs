using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
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
        private const int Timeout = 10 * 1000;

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

                switch (MatchData.Role)
                {
                    case Role.Host:
                        LoadingData.NextScene = "Host";
                        break;
                    case Role.Client:
                        LoadingData.NextScene = "Client";
                        break;
                }

                SceneManager.LoadScene("Loading");
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    _tokenSource.Cancel();
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    _errorPanel.SetActive(true);
                    ShowError(e is HttpRequestException
                        ? "Произошла ошибка при попытке подключиться к серверу."
                        : e.Message);
                    _cancelButton.interactable = false;
                }

                MatchData.UdpClient?.Dispose();
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
            var port = await GetMatchPortAsync(privateEndPoint, config.Url, config.Token, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            ShowStatus("Получение данных...");
            var message = await GetConnectionMessageAsync(MatchData.UdpClient, config.Ip, port, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            MatchData.Role = message.Role;
            MatchData.SessionId = message.SessionId;

            ShowStatus("Соединение...");
            MatchData.Clients = await PunchHoleAsync(MatchData.UdpClient, MatchData.SessionId, message.Clients, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }

        private async Task<int> GetMatchPortAsync(
            ClientEndPoint privateEndPoint, 
            string host, 
            string bearerToken, 
            CancellationToken cancellationToken) 
            => await new MatchmakerConnector().ConnectAsync(privateEndPoint, host, bearerToken, cancellationToken);

        private async Task<ConnectionMessage> GetConnectionMessageAsync(
            UdpClient udpClient, 
            string ip, 
            int port, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await new MatchConnector()
                    .ConnectAsync(udpClient, ip, port, cancellationToken)
                    .GetResultOrThrowOnTimeOutAsync(Timeout);
            }
            catch (TimeoutException)
            {
                throw new TimeoutException("Истекло время на подключение к матчу.");
            }
        }

        private async Task<List<IPEndPoint>> PunchHoleAsync(
            UdpClient udpClient, 
            uint sessionId, 
            List<ClientEndPoints> clients, 
            CancellationToken cancellationToken)
        {
            try
            {
                return await new HolePuncher()
                    .ConnectAsync(udpClient, sessionId, clients, cancellationToken)
                    .GetResultOrThrowOnTimeOutAsync(Timeout);
            }
            catch (TimeoutException)
            {
                throw new TimeoutException("Истекло время на соединение.");
            }
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
