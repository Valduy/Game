using Assets.Scripts.ECS.Components;
using ECS.Core;

using System.Linq;
using UnityEngine;


namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class CalculateVelocitySystem : SystemBase
    {
        public class Node : NodeBase
        {
            public DirectionComponent DirectionComponent { get; private set; }
            public SpeedComponent SpeedComponent { get; private set; }
            public VelocityComponent VelocityComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                DirectionComponent = Entity.Get<DirectionComponent>();
                SpeedComponent = Entity.Get<SpeedComponent>();
                VelocityComponent = Entity.Get<VelocityComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                node.VelocityComponent.Velocity = node.DirectionComponent.Direction * node.SpeedComponent.Speed;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
