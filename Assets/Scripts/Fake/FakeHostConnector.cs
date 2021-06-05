using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.UI.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Fake
{
    public class FakeHostConnector : MonoBehaviour
    {
        void Start()
        {
            MatchData.UdpClient = new UdpClient(54322);
            MatchData.Clients = new List<IPEndPoint> { new IPEndPoint(IPAddress.Loopback, 54321) };
            SceneManager.LoadScene("Host");
        }
    }
}
