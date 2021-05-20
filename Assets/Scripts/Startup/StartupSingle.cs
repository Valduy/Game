using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Systems.Fixed;
using Assets.Scripts.ECS.Systems.Unfixed;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public class StartupSingle : StartupBase
    {
        public GameObject PlayerPrefab;

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
            var playerEntity = EntityHelper.GetPlayerEntity(PlayerPrefab, 3);

            Unfixed.AddEntity(playerEntity);
            Fixed.AddEntity(playerEntity);
        }
    }
}
