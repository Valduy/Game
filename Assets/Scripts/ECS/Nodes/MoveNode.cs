using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class MoveNode : NodeBase
    {
        public SpeedComponent SpeedComponent { get; private set; }
        public DirectionComponent DirectionComponent { get; private set; }
        public RigidbodyComponent RigidbodyComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            SpeedComponent = Entity.Get<SpeedComponent>();
            DirectionComponent = Entity.Get<DirectionComponent>();
            RigidbodyComponent = Entity.Get<RigidbodyComponent>();
        }
    }
}
