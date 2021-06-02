using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Network.Messages;

namespace Assets.Scripts.UI.Loading
{
    public static class MatchData
    {
        public static UdpClient UdpClient { get; set; }
        public static Role Role { get; set; }
        public static List<IPEndPoint> Clients;
    }
}
