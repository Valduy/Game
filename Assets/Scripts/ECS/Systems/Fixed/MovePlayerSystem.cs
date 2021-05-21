using Assets.Scripts.ECS.Nodes;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class MovePlayerSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<MoveNode>())
            {
                node.RigidbodyComponent.Rigidbody.velocity = node.VelocityComponent.Velocity;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
