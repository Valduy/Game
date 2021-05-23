using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class KillCharacterNode : NodeBase
    {
        public HealthComponent HealthComponent { get; private set; }
        public MoveEnableComponent MoveEnableComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            HealthComponent = Entity.Get<HealthComponent>();
            MoveEnableComponent = Entity.Get<MoveEnableComponent>();
        }
    }
}
