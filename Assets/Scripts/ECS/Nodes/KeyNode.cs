using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class KeyNode : NodeBase
    {
        public InputReceiverComponent InputReceiverComponent { get; private set; }
        public KeyComponent KeyComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            InputReceiverComponent = Entity.Get<InputReceiverComponent>();
            KeyComponent = Entity.Get<KeyComponent>();
        }
    }
}
