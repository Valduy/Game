using System.Net;
using System.Net.Sockets;
using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Unfixed;
using Network.Proxy;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupClient : StartupBase
    {
        private UdpClient _udpClient;
        private IPEndPoint _hostAddress;
        private ClientNetworkProxy _clientProxy;
        private Reconcilator _reconcilator;

        public GameObject PlayerPrefab;
        public GameObject CameraPrefab;

        protected override void Start()
        {
            base.Start();

            _udpClient = new UdpClient(54321);
            _hostAddress = new IPEndPoint(IPAddress.Loopback, 54322);
            _clientProxy = new ClientNetworkProxy(_udpClient, _hostAddress);
            _clientProxy.Start();
            _reconcilator = new Reconcilator(Fixed, _clientProxy);

            AddUnfixedSystems(new CollectKeyInputsSystem());

            AddFixedSystems(
                new ApplyPositionSystem(),
                new MoveCameraSystem(),
                new ResetKeysInputsSystem());

            var thisPlayerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var thisPlayerEntity = EntityHelper.GetThisPlayerPhantomEntity(thisPlayerGo, 1, 3);

            var cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            var cameraEntity = EntityHelper.GetCamera(cameraGo, thisPlayerGo);

            var otherPlayerGo = Instantiate(PlayerPrefab, new Vector3(3, 3, 0), Quaternion.identity);
            var otherPlayerEntity = EntityHelper.GetOtherPlayerPhantomEntity(otherPlayerGo, 0, 3);

            Unfixed.AddEntity(thisPlayerEntity);
            Unfixed.AddEntity(otherPlayerEntity);

            Fixed.AddEntity(thisPlayerEntity);
            Fixed.AddEntity(otherPlayerEntity);
            Fixed.AddEntity(cameraEntity);
        }

        protected override void FixedUpdate()
        {
            _reconcilator.Update(Time.fixedDeltaTime);
        }

        void OnDestroy()
        {
            _udpClient?.Dispose();
            _clientProxy?.Dispose();
        }
    }
}
