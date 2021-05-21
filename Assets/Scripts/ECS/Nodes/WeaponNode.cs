using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    class WeaponNode : NodeBase
    {
        public MouseComponent MouseComponent { get; private set; }
        public TransformComponent TransformComponent { get; private set; }
        public OwnerTransformComponent OwnerTransformComponent { get; private set; }
        public WeaponRadiusComponent WeaponRadiusComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            MouseComponent = Entity.Get<MouseComponent>();
            TransformComponent = Entity.Get<TransformComponent>();
            OwnerTransformComponent = Entity.Get<OwnerTransformComponent>();
            WeaponRadiusComponent = Entity.Get<WeaponRadiusComponent>();
        }
    }
}
