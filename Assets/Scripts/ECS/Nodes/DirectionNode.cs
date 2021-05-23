using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class DirectionNode : NodeBase
    {
        public DirectionComponent DirectionComponent { get; set; }
        public KeysComponent KeysComponent { get; set; }
        public MoveEnableComponent MoveEnableComponent { get; set; }

        protected override void OnEntityChanged()
        {
            DirectionComponent = Entity.Get<DirectionComponent>();
            KeysComponent = Entity.Get<KeysComponent>();
            MoveEnableComponent = Entity.Get<MoveEnableComponent>();
        }
    }
}
