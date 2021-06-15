﻿using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Util;
using Connectors.HolePuncher;
using Connectors.MatchConnectors;
using Network.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Loading
{
    public class HolePunching : MonoBehaviour
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
                await PunchHole(_tokenSource.Token);

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
            catch (OperationCanceledException)
            {
                MatchData.UdpClient?.Dispose();
                SceneManager.LoadScene("MainMenu");
            }
            catch (Exception e)
            {
                _errorPanel.SetActive(true);
                ShowError(e is HttpRequestException
                    ? "Произошла ошибка при попытке подключиться к серверу."
                    : e.Message);
                _cancelButton.interactable = false;
            }
        }

        void OnDestroy()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }
        private async Task PunchHole(CancellationToken cancellationToken)
        {
            MatchData.UdpClient = new UdpClient(0);
            var config = StreamingAssetsHelper.GetConfig();

            ShowStatus("Получение данных...");
            var matchConnector = new MatchConnector();
            var message = await matchConnector.ConnectAsync(MatchData.UdpClient, config.Ip, 54321, cancellationToken);
            MatchData.Role = message.Role;
            cancellationToken.ThrowIfCancellationRequested();

            ShowStatus("Соединение...");
            var holePuncher = new HolePuncher();
            MatchData.Clients = await holePuncher.ConnectAsync(MatchData.UdpClient, message.SessionId, message.Clients, cancellationToken);
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
