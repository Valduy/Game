using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Unfixed;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupSingle : StartupBase
    {
        public GameObject CameraPrefab;
        public GameObject PlayerPrefab;
        public GameObject SwordPrefab;
        public GameObject BossPrefab;

        private GameObject _cameraGo;
        private GameObject _playerGo;
        private GameObject _swordGo;
        private GameObject _bossGo;

        private Entity _cameraEntity;
        private Entity _playerEntity;
        private Entity _swordEntity;
        private Entity _bossEntity;

        protected override void Start()
        {
            base.Start();

            AddUnfixedSystems(
                new CollectKeyInputsSystem(), 
                new CollectMouseInputsSystem());

            AddFixedSystems(
                new ResetDirectionSystem(),
                new DirectionSystem(),
                new VelocitySystem(),
                new MoveCharacterSystem(),
                new MoveWeaponSystem(),
                new MoveCameraSystem(),
                new ResetKeysInputsSystem());

            InstantiateGameObjects();
            CreateEntities();
            RegisterEntities();
        }

        private void InstantiateGameObjects()
        {
            _cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);

            _playerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _swordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _swordGo.transform.parent = _playerGo.transform;
            
            _bossGo = Instantiate(BossPrefab, new Vector3(0, 5, 0), Quaternion.identity);
        }

        private void CreateEntities()
        {
            _cameraEntity = EntityHelper.GetCameraEntity(_cameraGo, _playerGo);

            _playerEntity = EntityHelper.GetPlayerEntity(_playerGo, 3)
                .KeyInputsReceiver();

            _swordEntity = EntityHelper.GetWeaponEntity(_swordGo, _playerGo, 1.2f)
                .MouseInputReceiver(_cameraGo);

            _bossEntity = EntityHelper.GetBossEntity(_bossGo, 2.5f);
        }

        private void RegisterEntities()
        {
            Unfixed.AddEntity(_playerEntity);
            Unfixed.AddEntity(_swordEntity);

            Fixed.AddEntity(_playerEntity);
            Fixed.AddEntity(_swordEntity);
            Fixed.AddEntity(_cameraEntity);
            Fixed.AddEntity(_bossEntity);
        }
    }
}
