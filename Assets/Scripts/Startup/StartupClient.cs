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
        public GameObject SwordPrefab;

        protected override void Start()
        {
            base.Start();

            _udpClient = new UdpClient(54321);
            _hostAddress = new IPEndPoint(IPAddress.Loopback, 54322);
            _clientProxy = new ClientNetworkProxy(_udpClient, _hostAddress);
            _clientProxy.Start();
            _reconcilator = new Reconcilator(Fixed, _clientProxy);

            AddUnfixedSystems(
                new CollectKeyInputsSystem(), 
                new CollectMouseInputsSystem());

            AddFixedSystems(
                new ApplyPositionSystem(),
                new ApplyRotationSystem(),
                new MoveCameraSystem(),
                new ResetKeysInputsSystem());

            var thisPlayerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var thisPlayerSwordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            thisPlayerSwordGo.transform.parent = thisPlayerGo.transform;
            var cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            
            var otherPlayerGo = Instantiate(PlayerPrefab, new Vector3(3, 3, 0), Quaternion.identity);
            var otherPlayerSwordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            otherPlayerSwordGo.transform.parent = otherPlayerGo.transform;

            var thisPlayerEntity = EntityHelper.GetClientPlayerEntity(thisPlayerGo, 2, 3)
                .KeyInputsReceiver()
                .Serializable();

            var thisPlayerSword = EntityHelper.GetClientSwordEntity(thisPlayerSwordGo, 3)
                .MouseInputReceiver(cameraGo)
                .Serializable();

            var cameraEntity = EntityHelper.GetCameraEntity(cameraGo, thisPlayerGo);

            var otherPlayerEntity = EntityHelper.GetClientPlayerEntity(otherPlayerGo, 0, 3);
            var otherPlayerSwordEntity = EntityHelper.GetClientSwordEntity(otherPlayerSwordGo, 1);

            Unfixed.AddEntity(thisPlayerEntity);
            Unfixed.AddEntity(thisPlayerSword);
            Unfixed.AddEntity(otherPlayerEntity);

            Fixed.AddEntity(thisPlayerEntity);
            Fixed.AddEntity(thisPlayerSword);
            Fixed.AddEntity(cameraEntity);
            Fixed.AddEntity(otherPlayerEntity);
            Fixed.AddEntity(otherPlayerSwordEntity);
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
