using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Late
{
    public class MoveCameraSystem : SystemBase
    {
        public class Node : NodeBase
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

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                node.TransformComponent.Transform.position = new Vector3(
                    node.OwnerTransformComponent.Transform.position.x,
                    node.OwnerTransformComponent.Transform.position.y,
                    node.TransformComponent.Transform.position.z);
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
