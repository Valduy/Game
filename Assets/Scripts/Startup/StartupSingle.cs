using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Late;
using Assets.Scripts.ECS.Systems.Unfixed;
using ECS.Core;
using UnityEngine;

using Assets.Scripts.ECS.Components;

namespace Assets.Scripts.Startup
{
    public class StartupSingle : StartupBase
    {
        public GameObject CameraPrefab;
        public GameObject PlayerPrefab;
        public GameObject SwordPrefab;
        public GameObject BossPrefab;

        public GameObject GameOverPrefab;
        public GameObject VictoryPrefab;
        public GameObject MenuPrefab;

        private GameObject _cameraGo;
        private GameObject _playerGo;
        private GameObject _swordGo;
        private GameObject _bossGo;
        private GameObject _swordBossGO;

        private Entity _cameraEntity;
        private Entity _playerEntity;
        private Entity _swordEntity;
        private Entity _bossEntity;
        private Entity _swordBossEntity;

        private Entity _menuEntity;

        protected override void Start()
        {
            base.Start();

            AddUnfixedSystems(
                new CollectKeyInputsSystem(), 
                new CollectMouseInputsSystem());

            AddFixedSystems(
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
                new ResetKeysInputsSystem(),
                new EndGameSystem());

            AddLateSystem(
                new MoveCameraSystem(), 
                new UpdateHealthBarSystem());

            InstantiateGameObjects();
            CreateEntities();
            RegisterEntities();
        }

        private void InstantiateGameObjects()
        {
            _cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);

            _playerGo = Instantiate(PlayerPrefab, new Vector3(0, 13, -9), Quaternion.identity);
            _swordGo = Instantiate(SwordPrefab, new Vector3(0, 13, -9), Quaternion.identity);
            _swordGo.transform.parent = _playerGo.transform;
            
            _bossGo = Instantiate(BossPrefab, new Vector3(0, 0, -9), Quaternion.identity);

            _swordBossGO = Instantiate(SwordPrefab, new Vector3(0, 0, -9), Quaternion.identity);
            _swordBossGO.transform.parent = _bossGo.transform;
        }

        private void CreateEntities()
        {
            _cameraEntity = EntityHelper.GetCameraEntity(_cameraGo, _playerGo);

            _playerEntity = EntityHelper.GetCharacterEntity(_playerGo, 100, 3)
                .KeyInputsReceiver()
                .PlayerIdentity()
                .SetAnimatable(_playerGo.GetComponent<Animator>());

            _swordEntity = EntityHelper.GetWeaponEntity(_swordGo, _playerGo, _playerEntity, 1.2f, 1, 1)
                .MouseInputReceiver(_cameraGo);

            _bossEntity = EntityHelper.GetCharacterEntity(_bossGo, 400, 2.5f)
                .EnemyIdentity(2)
                .GoalIndigentIdentity(new Entity[] { _playerEntity })
                .SetAnimatable(_bossGo.GetComponent<Animator>());

            _swordBossEntity = EntityHelper.GetWeaponEntity(_swordBossGO, _bossGo, _bossEntity, 2f, 1, 1)
                .VirtualMouse(4, 100)
                .EnemyWeaponIdentity();

            _bossEntity.Add(new ItsWeaponEntityComponent() { Weapon = _swordBossEntity });

            _menuEntity = EntityHelper.GetMenuEntity(GameOverPrefab, VictoryPrefab, MenuPrefab);
        }

        private void RegisterEntities()
        {
            Unfixed.AddEntity(_playerEntity);
            Unfixed.AddEntity(_swordEntity);

            Fixed.AddEntity(_playerEntity);
            Fixed.AddEntity(_swordEntity);
            Fixed.AddEntity(_bossEntity);
            Fixed.AddEntity(_swordBossEntity);
            Fixed.AddEntity(_menuEntity);

            Late.AddEntity(_cameraEntity);
            Late.AddEntity(_playerEntity);
            Late.AddEntity(_bossEntity);
        }
    }
}
