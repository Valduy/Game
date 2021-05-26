using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Late
{
    public class UpdateHealthBarSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public HealthComponent HealthComponent { get; private set; }
            public HealthBarComponent HealthBarComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                HealthComponent = Entity.Get<HealthComponent>();
                HealthBarComponent = Entity.Get<HealthBarComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                node.HealthBarComponent.HealthBar.MaxHealth = node.HealthComponent.MaxHealth;
                node.HealthBarComponent.HealthBar.CurrentHealth = node.HealthComponent.CurrentHealth;
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
