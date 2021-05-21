using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Unfixed;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupSingle : StartupBase
    {
        public GameObject PlayerPrefab;
        public GameObject SwordPrefab;

        protected override void Start()
        {
            base.Start();

            AddUnfixedSystems(new CollectInputsSystem());

            AddFixedSystems(
                new ResetDirectionSystem(),
                new DirectionSystem(),
                new MoveSystem(),
                new ResetInputsSystem());

            var player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var playerEntity = EntityHelper.GetPlayerEntity(player, 3);

            var sword = Instantiate(SwordPrefab, new Vector3(1.2f, 0, 0), Quaternion.identity);
            sword.transform.parent = player.transform;
            //var swordEntity

            Unfixed.AddEntity(playerEntity);
            Fixed.AddEntity(playerEntity);
        }
    }
}
