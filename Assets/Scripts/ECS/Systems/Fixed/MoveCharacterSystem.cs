using Assets.Scripts.ECS.Components;
using ECS.Core;

using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class MoveCharacterSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public VelocityComponent VelocityComponent { get; private set; }
            public RigidbodyComponent RigidbodyComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                VelocityComponent = Entity.Get<VelocityComponent>();
                RigidbodyComponent = Entity.Get<RigidbodyComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                node.RigidbodyComponent.Rigidbody.velocity = node.VelocityComponent.Velocity;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
