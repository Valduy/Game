using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class StoreWeaponPreviousAngleSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public WeaponPreviousAngleComponent WeaponPreviousAngleComponent { get; private set; }
            public TransformComponent TransformComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                WeaponPreviousAngleComponent = Entity.Get<WeaponPreviousAngleComponent>();
                TransformComponent = Entity.Get<TransformComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                node.WeaponPreviousAngleComponent.PreviousAngle = node.TransformComponent.Transform.rotation.eulerAngles.z;
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
