using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class WeaponDamageNode : NodeBase
    {
        public ColliderComponent ColliderComponent { get; private set; }
        public DamageComponent DamageComponent { get; private set; }
        public PreviousWeaponAngleComponent PreviousWeaponAngleComponent { get; private set; }
        public WeaponEffectiveDeltaComponent WeaponEffectiveDeltaComponent { get; private set; }
        public TransformComponent TransformComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            ColliderComponent = Entity.Get<ColliderComponent>();
            DamageComponent = Entity.Get<DamageComponent>();
            PreviousWeaponAngleComponent = Entity.Get<PreviousWeaponAngleComponent>();
            WeaponEffectiveDeltaComponent = Entity.Get<WeaponEffectiveDeltaComponent>();
            TransformComponent = Entity.Get<TransformComponent>();
        }
    }
}
