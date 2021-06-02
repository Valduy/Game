using System;
using System.Collections;
using System.Collections.Generic;
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
        private int _matchPort;
        private ConnectionMessage _connectionMessage;

        public Button CancelButton;
        public TMP_Text StatusText;

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
            catch (Exception e)
            {
                // TODO: ловить более конкретные исключения
                Debug.Log(e.Message);
                StatusText.GetComponent<TextMeshProUGUI>().text = e.StackTrace;
                CancelButton.interactable = false;
            }

            //_tokenSource = new CancellationTokenSource();

            //this.StartThrowingCoroutine(GetMatchCoroutine(_tokenSource.Token), e =>
            //{
            //    if (e == null)
            //    {
            //        Debug.Log("Все хорошо.");
            //    }
            //    else if (e is OperationCanceledException)
            //    {
            //        MatchData.UdpClient?.Dispose();
            //        SceneManager.LoadScene("MainMenu");
            //    }
            //    else
            //    {
            //        Debug.Log(e.Message);
            //        CancelButton.interactable = false;
            //    }
            //});
        }

        void OnDestroy()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }

        //private IEnumerator GetMatchCoroutine(CancellationToken cancellationToken)
        //{
        //    MatchData.UdpClient = new UdpClient(0);
        //    var config = StreamingAssetsHelper.GetConfig();

        //    yield return SearchMatchCoroutine(config.Url, config.Token, cancellationToken);
        //    yield return GetMatchInfoCoroutine(config.Ip, _matchPort, cancellationToken);
        //    yield return HolePunchingCoroutine(cancellationToken);
        //}

        //private IEnumerator SearchMatchCoroutine(string url, string token, CancellationToken cancellationToken)
        //{
        //    Debug.Log("ждем матч");
        //    var privateEndPoint = new ClientEndPoint(NetworkHelper.GetLocalIPAddress(), MatchData.UdpClient.GetPort());
        //    var matchmakerConnector = new MatchmakerConnector();
        //    yield return matchmakerConnector
        //        .ConnectAsync(privateEndPoint, url, token, cancellationToken)
        //        .ToCoroutine(t =>
        //        {
        //            ThrowIfFaultedOrCanceled(t);
        //            _matchPort = t.Result;
        //        });
        //}

        //private IEnumerator GetMatchInfoCoroutine(string ip, int port, CancellationToken cancellationToken)
        //{
        //    Debug.Log("получаем информацию");
        //    var matchConnector = new MatchConnector();
        //    yield return matchConnector
        //        .ConnectAsync(MatchData.UdpClient, ip, port, cancellationToken)
        //        .ToCoroutine(t =>
        //        {
        //            ThrowIfFaultedOrCanceled(t);
        //            _connectionMessage = t.Result;
        //        });
        //}

        //private IEnumerator HolePunchingCoroutine(CancellationToken cancellationToken)
        //{
        //    Debug.Log("соединяемся");
        //    var holePuncher = new HolePuncher();
        //    yield return holePuncher
        //        .ConnectAsync(MatchData.UdpClient, _connectionMessage.Clients, cancellationToken)
        //        .ToCoroutine(t =>
        //        {
        //            ThrowIfFaultedOrCanceled(t);
        //            MatchData.Clients = t.Result;
        //        });
        //}

        private async Task SearchMatchAsync(CancellationToken cancellationToken)
        {
            MatchData.UdpClient = new UdpClient(0);
            var config = StreamingAssetsHelper.GetConfig();
            var privateEndPoint = new ClientEndPoint(NetworkHelper.GetLocalIPAddress(), MatchData.UdpClient.GetPort());

            //StatusText.GetComponent<TextMeshProUGUI>().text = "Поиск матча...";
            var matchmakerConnector = new MatchmakerConnector();
            var port = await matchmakerConnector.ConnectAsync(privateEndPoint, config.Url, config.Token, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            //StatusText.GetComponent<TextMeshProUGUI>().text = "Получение данных...";
            var matchConnector = new MatchConnector();
            var message = await matchConnector.ConnectAsync(MatchData.UdpClient, config.Ip, port, cancellationToken);
            MatchData.Role = message.Role;
            cancellationToken.ThrowIfCancellationRequested();

            //StatusText.GetComponent<TextMeshProUGUI>().text = "соединение...";
            var holePuncher = new HolePuncher();
            MatchData.Clients = await holePuncher.ConnectAsync(MatchData.UdpClient, message.Clients, cancellationToken);
        }

        //private void ThrowIfFaultedOrCanceled(Task task)
        //{
        //    if (task.IsCanceled) throw new OperationCanceledException();
        //    if (task.IsFaulted) throw task.Exception.InnerException;
        //}
    }
}
