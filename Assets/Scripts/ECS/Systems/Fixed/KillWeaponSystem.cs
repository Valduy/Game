using System.Linq;
using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Nodes;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class KillWeaponSystem :  SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<KillWeaponNode>().ToList())
            {
                if (node.OwnerHealthComponentComponent.HealthComponent.CurrentHealth <= 0)
                {
                    node.Entity.Remove<AttackEnableComponent>();
                }
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
