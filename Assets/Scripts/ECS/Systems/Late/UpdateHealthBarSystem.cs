using Assets.Scripts.ECS.Nodes;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Late
{
    public class UpdateHealthBarSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<HealthBarNode>())
            {
                node.HealthBarComponent.HealthBar.CurrentHealth = node.HealthComponent.Health;
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
