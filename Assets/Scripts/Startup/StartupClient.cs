using System.Net;
using System.Net.Sockets;
using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Unfixed;
using ECS.Core;
using Network.Proxy;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupClient : StartupBase
    {
        public GameObject PlayerPrefab;
        public GameObject CameraPrefab;
        public GameObject SwordPrefab;
        public GameObject BossPrefab;

        private UdpClient _udpClient;
        private IPEndPoint _hostAddress;
        private ClientNetworkProxy _clientProxy;
        private Reconcilator _reconcilator;

        private GameObject _cameraGo;
        private GameObject _thisPlayerGo;
        private GameObject _thisPlayerSwordGo;
        private GameObject _otherPlayerGo;
        private GameObject _otherPlayerSwordGo;

        private Entity _cameraEntity;
        private Entity _thisPlayerEntity;
        private Entity _thisPlayerSwordEntity;
        private Entity _otherPlayerEntity;
        private Entity _otherPlayerSwordEntity;

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

            InstantiateGameObjects();
            CreateEntities();
            RegisterEntities();
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

        private void InstantiateGameObjects()
        {
            _cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);

            _thisPlayerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _thisPlayerSwordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _thisPlayerSwordGo.transform.parent = _thisPlayerGo.transform;

            _otherPlayerGo = Instantiate(PlayerPrefab, new Vector3(3, 3, 0), Quaternion.identity);
            _otherPlayerSwordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _otherPlayerSwordGo.transform.parent = _otherPlayerGo.transform;
        }

        private void CreateEntities()
        {
            _cameraEntity = EntityHelper.GetCameraEntity(_cameraGo, _thisPlayerGo);

            _thisPlayerEntity = EntityHelper.GetClientPlayerEntity(_thisPlayerGo, 2, 3)
                .KeyInputsReceiver()
                .Serializable();

            _thisPlayerSwordEntity = EntityHelper.GetClientWeaponEntity(_thisPlayerSwordGo, 3)
                .MouseInputReceiver(_cameraGo)
                .Serializable();
            
            _otherPlayerEntity = EntityHelper.GetClientPlayerEntity(_otherPlayerGo, 0, 3);
            _otherPlayerSwordEntity = EntityHelper.GetClientWeaponEntity(_otherPlayerSwordGo, 1);
        }

        private void RegisterEntities()
        {
            Unfixed.AddEntity(_thisPlayerEntity);
            Unfixed.AddEntity(_thisPlayerSwordEntity);

            Fixed.AddEntity(_thisPlayerEntity);
            Fixed.AddEntity(_thisPlayerSwordEntity);
            Fixed.AddEntity(_cameraEntity);
            Fixed.AddEntity(_otherPlayerEntity);
            Fixed.AddEntity(_otherPlayerSwordEntity);
        }
    }
}
