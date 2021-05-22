using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class StorePreviousWeaponAngleSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<PreviousWeaponAngleNode>())
            {
                node.PreviousWeaponAngleComponent.PreviousAngle = node.TransformComponent.Transform.rotation.eulerAngles.z;
            }
        }

        public override void Register(Engine engine)
        {
            _engine = engine;
        }

        public override void Unregister(Engine engine)
        {
            _engine = null;
        }
    }
}
