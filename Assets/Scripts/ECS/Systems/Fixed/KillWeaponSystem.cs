using System.Linq;
using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class KillWeaponSystem :  SystemBase
    {
        public class Node : NodeBase
        {
            public OwnerHealthComponentComponent OwnerHealthComponentComponent { get; private set; }
            public IsAttackEnableComponent IsAttackEnableComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                OwnerHealthComponentComponent = Entity.Get<OwnerHealthComponentComponent>();
                IsAttackEnableComponent = Entity.Get<IsAttackEnableComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                if (node.OwnerHealthComponentComponent.HealthComponent.CurrentHealth <= 0)
                {
                    node.Entity.Remove<IsAttackEnableComponent>();
                }
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
