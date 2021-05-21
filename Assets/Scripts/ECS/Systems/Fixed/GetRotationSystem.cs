using Assets.Scripts.ECS.Nodes;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class GetRotationSystem : SystemBase
    {
        public Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<RotationNode>())
            {
                node.RotationComponent.Rotation = node.TransformComponent.Transform.rotation;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
