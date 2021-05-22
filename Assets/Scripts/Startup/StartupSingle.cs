using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Unfixed;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupSingle : StartupBase
    {
        public GameObject CameraPrefab;
        public GameObject PlayerPrefab;
        public GameObject SwordPrefab;
        public GameObject BossPrefab;

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

            var playerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var swordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            swordGo.transform.parent = playerGo.transform;
            var cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);

            var bossGo = Instantiate(BossPrefab, new Vector3(0, 5, 0), Quaternion.identity);

            var playerEntity = EntityHelper.GetPlayerEntity(playerGo, 3)
                .KeyInputsReceiver();

            var swordEntity = EntityHelper.GetWeaponEntity(swordGo, playerGo, 1.2f)
                .MouseInputReceiver(cameraGo);

            var cameraEntity = EntityHelper.GetCameraEntity(cameraGo, playerGo);

            var bossEntity = EntityHelper.GetBossEntity(bossGo, 2.5f);

            Unfixed.AddEntity(playerEntity);
            Unfixed.AddEntity(swordEntity);

            Fixed.AddEntity(playerEntity);
            Fixed.AddEntity(swordEntity);
            Fixed.AddEntity(cameraEntity);
            Fixed.AddEntity(bossEntity);
        }
    }
}
