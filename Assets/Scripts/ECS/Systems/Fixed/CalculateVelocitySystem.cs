using Assets.Scripts.ECS.Nodes;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class CalculateVelocitySystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<VelocityNode>())
            {
                node.VelocityComponent.Velocity = node.DirectionComponent.Direction * node.SpeedComponent.Speed;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
