using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
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
    public class StartupHost : StartupBase
    {
        public GameObject CameraPrefab;
        public GameObject PlayerPrefab;
        public GameObject SwordPrefab;
        public GameObject SwordBossPrefab;
        public GameObject BossPrefab;

        public GameObject GameOverPrefab;
        public GameObject VictoryPrefab;
        public GameObject MenuPrefab;

        private UdpClient _udpClient;
        private List<IPEndPoint> _clients;
        private HostNetworkProxy _hostProxy;
        private Snapshoter _snapshoter;

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
            _clients = MatchData.Clients;
            _hostProxy = new HostNetworkProxy(_udpClient, MatchData.SessionId, _clients);
            _hostProxy.Start();
            _snapshoter = new Snapshoter(Fixed, _hostProxy);
            
            AddUnfixedSystems(
                new CollectKeyInputsSystem(), 
                new CollectMouseInputsSystem());

            AddFixedSystems(
                new CollectKeyInputsSystem(),
                new CollectMouseInputsSystem(),
                new ResetDirectionSystem(),

                new AvailableGoalsConfiguratorSystem(),
                new GoalFindSystem(),
                new TrackGoalSystem(),
                new CheckBossDangerZoneSystem(),
                new AttackPreparationSystem(),
                new VirtualMouseSystem(),
                new VirtualMouseToMouseConverterSystem(),

                new CalculateDirectionSystem(),
                new CalculateVelocitySystem(),
                new MoveCharacterSystem(),

                new AnimationSystem(),

                new StoreWeaponPreviousAngleSystem(),
                new MoveWeaponSystem(),
                new DamageByWeaponSystem(),
                new KillCharacterSystem(),
                new KillWeaponSystem(),
                new GetPositionSystem(),
                new GetRotationSystem(),
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
            _snapshoter.Update(Time.fixedDeltaTime);
        }

        void OnDestroy()
        {
            _udpClient?.Dispose();
            _hostProxy?.Dispose();
        }

        private void InstantiateGameObjects()
        {
            _cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);

            _thisPlayerGo = Instantiate(PlayerPrefab, new Vector3(3, 3, 0), Quaternion.identity);
            _thisPlayerSwordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _thisPlayerSwordGo.transform.parent = _thisPlayerGo.transform;

            _otherPlayerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _otherPlayerSwordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _otherPlayerSwordGo.transform.parent = _otherPlayerGo.transform;

            _bossGo = Instantiate(BossPrefab, new Vector3(0, 5, 0), Quaternion.identity);

            _swordBossGO = Instantiate(SwordBossPrefab, new Vector3(0, 0, -9), Quaternion.identity);
            _swordBossGO.transform.parent = _bossGo.transform;
        }

        private void CreateEntities()
        {
            _cameraEntity = EntityHelper.GetCameraEntity(_cameraGo, _thisPlayerGo);

            _thisPlayerEntity = EntityHelper.GetHostCharacterEntity(_thisPlayerGo, 0, 100, 3)
                .KeyInputsReceiver()
                .Serializable()
                .PlayerIdentity()
                .SetAnimatable(_thisPlayerGo.GetComponent<Animator>());

            _thisPlayerSwordEntity = EntityHelper.GetHostWeaponEntity(
                    _thisPlayerSwordGo,
                    _thisPlayerGo,
                    _thisPlayerEntity,
                    1, 1.2f, 1, 1)
                .MouseInputReceiver(_cameraGo)
                .Serializable();

            _otherPlayerEntity = EntityHelper.GetHostCharacterEntity(_otherPlayerGo, 2, 100, 3)
                .KeyInputSource()
                .Serializable()
                .PlayerIdentity() // Не уверен нужно ли это тут. Используется в EndGameSystem
                .SetAnimatable(_otherPlayerGo.GetComponent<Animator>());

            _otherPlayerSwordEntity = EntityHelper.GetHostWeaponEntity(
                    _otherPlayerSwordGo,
                    _otherPlayerGo,
                    _otherPlayerEntity,
                    3, 1.2f, 1, 1)
                .MouseInputSource()
                .Serializable();

            _bossEntity = EntityHelper.GetHostCharacterEntity(_bossGo, 4, 300, 2.5f)
                .Serializable()
                .EnemyIdentity()
                .SetDangerZone(2)
                .GoalIndigentIdentity(new Entity[] { _thisPlayerEntity, _otherPlayerEntity })
                .SetAnimatable(_bossGo.GetComponent<Animator>());

            _swordBossEntity = EntityHelper.GetHostWeaponEntity(
                    _swordBossGO, 
                    _bossGo, 
                    _bossEntity, 
                    5, 2f, 1, 1)
                .VirtualMouse(4, 100)
                .EnemyWeaponIdentity()
                .Serializable();

            _bossEntity.Add(new ItsWeaponEntityComponent() { Weapon = _swordBossEntity });

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
