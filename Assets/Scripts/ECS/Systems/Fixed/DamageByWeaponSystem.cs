using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class DamageByWeaponSystem : SystemBase
    {
        public class DamageNode : NodeBase
        {
            public ColliderComponent ColliderComponent { get; private set; }
            public DamageComponent DamageComponent { get; private set; }
            public WeaponPreviousAngleComponent PreviousWeaponAngleComponent { get; private set; }
            public WeaponEffectiveDeltaComponent WeaponEffectiveDeltaComponent { get; private set; }
            public TransformComponent TransformComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                ColliderComponent = Entity.Get<ColliderComponent>();
                DamageComponent = Entity.Get<DamageComponent>();
                PreviousWeaponAngleComponent = Entity.Get<WeaponPreviousAngleComponent>();
                WeaponEffectiveDeltaComponent = Entity.Get<WeaponEffectiveDeltaComponent>();
                TransformComponent = Entity.Get<TransformComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var weaponNode in _engine.GetNodes<DamageNode>())
            {
                if (IsAttack(weaponNode))
                {
                    foreach (var damagableNode in _engine.GetNodes<DamagableNode>())
                    {
                        if (IsIntersects(weaponNode.ColliderComponent, damagableNode.ColliderComponent))
                        {
                            damagableNode.HealthComponent.CurrentHealth -= weaponNode.DamageComponent.Damage;
                            damagableNode.HealthComponent.CurrentHealth = Mathf.Max(damagableNode.HealthComponent.CurrentHealth, 0);
                        }
                    }
                }
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

        private bool IsAttack(DamageNode weaponNode)
        {
            var delta = Mathf.Abs(weaponNode.PreviousWeaponAngleComponent.PreviousAngle -
                weaponNode.TransformComponent.Transform.rotation.eulerAngles.z);
            return delta >= weaponNode.WeaponEffectiveDeltaComponent.Delta;
        }

        private bool IsIntersects(ColliderComponent a, ColliderComponent b) 
            => Physics2D.IsTouching(a.Collider, b.Collider);
    }
}
