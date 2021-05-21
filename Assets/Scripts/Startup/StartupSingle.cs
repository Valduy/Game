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
                new MoveSystem(),
                new MoveWeaponSystem(),
                new ResetKeysInputsSystem());

            var cameraGo = Instantiate(CameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);

            var playerGo = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var playerEntity = EntityHelper.GetPlayerEntity(playerGo, 3);

            var swordGo = Instantiate(SwordPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            swordGo.transform.parent = playerGo.transform;
            var swordEntity = EntityHelper.GetSword(swordGo, playerGo, cameraGo, 1.2f);

            Unfixed.AddEntity(playerEntity);
            Unfixed.AddEntity(swordEntity);

            Fixed.AddEntity(playerEntity);
            Fixed.AddEntity(swordEntity);
        }
    }
}
