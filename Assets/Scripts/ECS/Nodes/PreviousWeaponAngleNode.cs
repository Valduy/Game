using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class PreviousWeaponAngleNode : NodeBase
    {
        public PreviousWeaponAngleComponent PreviousWeaponAngleComponent { get; private set; }
        public TransformComponent TransformComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            PreviousWeaponAngleComponent = Entity.Get<PreviousWeaponAngleComponent>();
            TransformComponent = Entity.Get<TransformComponent>();
        }
    }
}
