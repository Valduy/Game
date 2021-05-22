using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class DamagableNode : NodeBase
    {
        public ColliderComponent ColliderComponent { get; private set; }
        public HealthComponent HealthComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            ColliderComponent = Entity.Get<ColliderComponent>();
            HealthComponent = Entity.Get<HealthComponent>();
        }
    }
}
