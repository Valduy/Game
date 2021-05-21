using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Unfixed;
using Network.Proxy;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupHost : StartupBase
    {
        private UdpClient _udpClient;
        private List<IPEndPoint> _clients;
        private HostNetworkProxy _hostProxy;
        private Snapshoter _snapshoter;

        public GameObject PlayerPrefab;

        protected override void Start()
        {
            base.Start();

            _udpClient = new UdpClient(54322);
            _clients = new List<IPEndPoint>() { new IPEndPoint(IPAddress.Loopback, 54321) };
            _hostProxy = new HostNetworkProxy(_udpClient, _clients);
            _hostProxy.Start();
            _snapshoter = new Snapshoter(Fixed, _hostProxy);
            
            AddUnfixedSystems(new CollectKeyInputsSystem());

            AddFixedSystems(
                new ResetDirectionSystem(),
                new DirectionSystem(),
                new VelocitySystem(),
                new MoveSystem(),
                new GetPositionSystem(),
                new ResetKeysInputsSystem());
            
            var thisPlayerGo = Instantiate(PlayerPrefab, new Vector3(3, 3, 0), Quaternion.identity);
            var thisPlayerEntity = EntityHelper.GetThisPlayerEntity(thisPlayerGo, 0, 3);

            var otherPlayerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var otherPlayerEntity = EntityHelper.GetOtherPlayerEntity(otherPlayerGo, 1, 3, _clients.First());

            Unfixed.AddEntity(thisPlayerEntity);
            Unfixed.AddEntity(otherPlayerEntity);
            
            Fixed.AddEntity(thisPlayerEntity);
            Fixed.AddEntity(otherPlayerEntity);
        }

        protected override void FixedUpdate()
        {
            _snapshoter.Update(Time.fixedDeltaTime);
        }

        void OnDestroy()
        {
            _udpClient?.Dispose();
            _hostProxy?.Dispose();
        }
    }
}
