using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class DamageByWeaponSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var weaponNode in _engine.GetNodes<WeaponDamageNode>())
            {
                if (IsAttack(weaponNode))
                {
                    foreach (var damagableNode in _engine.GetNodes<DamagableNode>())
                    {
                        if (IsIntersects(weaponNode.ColliderComponent, damagableNode.ColliderComponent))
                        {
                            damagableNode.HealthComponent.CurrentHealth -= weaponNode.DamageComponent.Damage;
                            damagableNode.HealthComponent.CurrentHealth = Mathf.Max(damagableNode.HealthComponent.CurrentHealth, 0);
                            Debug.Log(damagableNode.HealthComponent.CurrentHealth);
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

        private bool IsAttack(WeaponDamageNode weaponNode)
        {
            var delta = Mathf.Abs(weaponNode.PreviousWeaponAngleComponent.PreviousAngle -
                weaponNode.TransformComponent.Transform.rotation.eulerAngles.z);
            return delta >= weaponNode.WeaponEffectiveDeltaComponent.Delta;
        }

        private bool IsIntersects(ColliderComponent a, ColliderComponent b) 
            => Physics2D.IsTouching(a.Collider, b.Collider);
    }
}
