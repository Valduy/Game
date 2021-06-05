using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.UI.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Menus
{
    public class TestMenu : MonoBehaviour
    {
        public void StartClient()
        {
            MatchData.UdpClient = new UdpClient(54321);
            MatchData.Clients = new List<IPEndPoint> { new IPEndPoint(IPAddress.Loopback, 54322) };
            SceneManager.LoadScene("Client");
        }

        public void StartHost()
        {
            MatchData.UdpClient = new UdpClient(54322);
            MatchData.Clients = new List<IPEndPoint> { new IPEndPoint(IPAddress.Loopback, 54321) };
            SceneManager.LoadScene("Host");
        }
    }
}
