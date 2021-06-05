using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.UI.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Fake
{
    public class FakeClientConnector : MonoBehaviour
    {
        void Start()
        {
            MatchData.UdpClient = new UdpClient(54321);
            MatchData.Clients = new List<IPEndPoint> {new IPEndPoint(IPAddress.Loopback, 54322)};
            SceneManager.LoadScene("Client");
        }
    }
}
