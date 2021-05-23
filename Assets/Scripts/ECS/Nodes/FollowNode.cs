using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    class FollowNode : NodeBase
    {
        public FollowComponent FollowComponent { get; private set; }
        public TransformComponent TransformComponent { get; private set; }
        public OwnerTransformComponent OwnerTransformComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            FollowComponent = Entity.Get<FollowComponent>();
            TransformComponent = Entity.Get<TransformComponent>();
            OwnerTransformComponent = Entity.Get<OwnerTransformComponent>();
        }
    }
}
