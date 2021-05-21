using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class VelocityNode : NodeBase
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
}
