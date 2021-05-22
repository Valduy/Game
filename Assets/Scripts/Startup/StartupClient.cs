using System.Net;
using System.Net.Sockets;
using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Late;
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
        private GameObject _bossGo;

        private Entity _cameraEntity;
        private Entity _thisPlayerEntity;
        private Entity _thisPlayerSwordEntity;
        private Entity _otherPlayerEntity;
        private Entity _otherPlayerSwordEntity;
        private Entity _bossEntity;

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
                new ApplyRotationSystem(),
                new ApplyPositionSystem(),
                new MoveCameraSystem(),
                new ResetKeysInputsSystem());

            AddLateSystem(new MoveCameraSystem());

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

            _bossGo = Instantiate(BossPrefab, new Vector3(0, 5, 0), Quaternion.identity);
        }

        private void CreateEntities()
        {
            _cameraEntity = EntityHelper.GetCameraEntity(_cameraGo, _thisPlayerGo);

            _thisPlayerEntity = EntityHelper.GetClientCharacterEntity(_thisPlayerGo, 2)
                .KeyInputsReceiver()
                .Serializable();

            _thisPlayerSwordEntity = EntityHelper.GetClientWeaponEntity(_thisPlayerSwordGo, 3)
                .MouseInputReceiver(_cameraGo)
                .Serializable();
            
            _otherPlayerEntity = EntityHelper.GetClientCharacterEntity(_otherPlayerGo, 0);

            _otherPlayerSwordEntity = EntityHelper.GetClientWeaponEntity(_otherPlayerSwordGo, 1);

            _bossEntity = EntityHelper.GetClientCharacterEntity(_bossGo, 4);
        }

        private void RegisterEntities()
        {
            Unfixed.AddEntity(_thisPlayerEntity);
            Unfixed.AddEntity(_thisPlayerSwordEntity);

            Fixed.AddEntity(_thisPlayerEntity);
            Fixed.AddEntity(_thisPlayerSwordEntity);
            Fixed.AddEntity(_otherPlayerEntity);
            Fixed.AddEntity(_otherPlayerSwordEntity);
            Fixed.AddEntity(_bossEntity);

            Late.AddEntity(_cameraEntity);
        }
    }
}
