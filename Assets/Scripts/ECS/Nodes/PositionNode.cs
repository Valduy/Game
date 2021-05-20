using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class PositionNode : NodeBase
    {
        public PositionComponent PositionComponent { get; private set; }
        public TransformComponent TransformComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            PositionComponent = Entity.Get<PositionComponent>();
            TransformComponent = Entity.Get<TransformComponent>();
        }
    }
}
