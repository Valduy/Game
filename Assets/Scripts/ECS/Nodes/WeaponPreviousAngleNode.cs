using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class WeaponPreviousAngleNode : NodeBase
    {
        public WeaponPreviousAngleComponent WeaponPreviousAngleComponent { get; private set; }
        public TransformComponent TransformComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            WeaponPreviousAngleComponent = Entity.Get<WeaponPreviousAngleComponent>();
            TransformComponent = Entity.Get<TransformComponent>();
        }
    }
}
