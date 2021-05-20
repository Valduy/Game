using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    class DirectionNode : NodeBase
    {
        public DirectionComponent DirectionComponent { get; set; }
        public KeyComponent KeyComponent { get; set; }

        protected override void OnEntityChanged()
        {
            DirectionComponent = Entity.Get<DirectionComponent>();
            KeyComponent = Entity.Get<KeyComponent>();
        }
    }
}
