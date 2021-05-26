using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class ResetDirectionSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public DirectionComponent DirectionComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                DirectionComponent = Entity.Get<DirectionComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                node.DirectionComponent.Direction = Vector2.zero;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
