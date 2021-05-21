using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class RotationNode : NodeBase
    {
        public RotationComponent RotationComponent { get; private set; }
        public TransformComponent TransformComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            RotationComponent = Entity.Get<RotationComponent>();
            TransformComponent = Entity.Get<TransformComponent>();
        }
    }
}
