using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class HealthBarNode : NodeBase
    {
        public HealthComponent HealthComponent { get; private set; }
        public HealthBarComponent HealthBarComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            HealthComponent = Entity.Get<HealthComponent>();
            HealthBarComponent = Entity.Get<HealthBarComponent>();
        }
    }
}
