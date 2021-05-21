using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class MouseNode : NodeBase
    {
        public InputReceiverComponent InputReceiverComponent { get; private set; }
        public MouseComponent MouseComponent { get; private set; }
        public CameraComponent CameraComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            MouseComponent = Entity.Get<MouseComponent>();
            InputReceiverComponent = Entity.Get<InputReceiverComponent>();
            CameraComponent = Entity.Get<CameraComponent>();
        }
    }
}
