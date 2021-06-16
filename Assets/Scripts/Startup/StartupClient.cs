using System.Linq;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Late;
using Assets.Scripts.ECS.Systems.Unfixed;
using Assets.Scripts.UI.Loading;
using ECS.Core;
using Network.Proxy;
using UnityEngine;

using Assets.Scripts.ECS.Components;

namespace Assets.Scripts.Startup
{
    public class StartupClient : StartupBase
    {
        public GameObject PlayerPrefab;
        public GameObject CameraPrefab;
        public GameObject SwordPrefab;
        public GameObject BossPrefab;

        public GameObject GameOverPrefab;
        public GameObject VictoryPrefab;
        public GameObject MenuPrefab;

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
        private GameObject _swordBossGO;

        private Entity _cameraEntity;
        private Entity _thisPlayerEntity;
        private Entity _thisPlayerSwordEntity;
        private Entity _otherPlayerEntity;
        private Entity _otherPlayerSwordEntity;
        private Entity _bossEntity;
        private Entity _swordBossEntity;

        private Entity _menuEntity;

        protected override void Start()
        {
            base.Start();

            _udpClient = MatchData.UdpClient;
            _hostAddress = MatchData.Clients.First();
            _clientProxy = new ClientNetworkProxy(_udpClient, _hostAddress);
            _clientProxy.Start();
            _reconcilator = new Reconcilator(Fixed, _clientProxy);

            AddUnfixedSystems(
                new CollectKeyInputsSystem(), 
                new CollectMouseInputsSystem());

            AddFixedSystems(
                new ApplyRotationSystem(),
                new ApplyPositionSystem(),

                new AnimationSystem(),

                new ResetKeysInputsSystem(),
                
                new EndGameSystem());

            AddLateSystem(
                new MoveCameraSystem(), 
                new UpdateHealthBarSystem());

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

            _swordBossGO = Instantiate(SwordPrefab, new Vector3(0, 0, -9), Quaternion.identity);
            _swordBossGO.transform.parent = _bossGo.transform;
        }

        private void CreateEntities()
        {
            _cameraEntity = EntityHelper.GetCameraEntity(_cameraGo, _thisPlayerGo);

            _thisPlayerEntity = EntityHelper.GetClientCharacterEntity(_thisPlayerGo, 100, 2)
                .KeyInputsReceiver()
                .Serializable()
                .PlayerIdentity()
                .SetAnimatable(_thisPlayerGo.GetComponent<Animator>());


            _thisPlayerSwordEntity = EntityHelper.GetClientWeaponEntity(_thisPlayerSwordGo, 3)
                .MouseInputReceiver(_cameraGo)
                .Serializable();

            _otherPlayerEntity = EntityHelper.GetClientCharacterEntity(_otherPlayerGo, 100, 0)
                .PlayerIdentity() // Не уверен нужно ли это тут. Используется в EndGameSystem
                .SetAnimatable(_otherPlayerGo.GetComponent<Animator>());

            _otherPlayerSwordEntity = EntityHelper.GetClientWeaponEntity(_otherPlayerSwordGo, 1);

            _bossEntity = EntityHelper.GetClientCharacterEntity(_bossGo, 100, 4)
                .EnemyIdentity() // Не уверен нужно ли это тут. Используется в EndGameSystem
                .SetAnimatable(_bossGo.GetComponent<Animator>());

            _swordBossEntity = EntityHelper.GetWeaponEntity(_swordBossGO, _bossGo, _bossEntity, 2f, 1, 1)
                .EnemyWeaponIdentity();

            // Этого вроде бы быть не должно, т.к. ItsWeaponEntityComponent используется только в VirtualMouse,
            // Но на всякий случай оставил
            //_bossEntity.Add(new ItsWeaponEntityComponent() { Weapon = _swordBossEntity });

            _menuEntity = EntityHelper.GetMenuEntity(GameOverPrefab, VictoryPrefab, MenuPrefab);
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
            Fixed.AddEntity(_swordBossEntity);
            Fixed.AddEntity(_menuEntity);

            Late.AddEntity(_cameraEntity);
            Late.AddEntity(_thisPlayerEntity);
            Late.AddEntity(_otherPlayerEntity);
            Late.AddEntity(_bossEntity);
        }
    }
}
