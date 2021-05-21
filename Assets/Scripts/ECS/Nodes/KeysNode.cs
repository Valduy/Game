using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class KeysNode : NodeBase
    {
        public InputReceiverComponent InputReceiverComponent { get; private set; }
        public KeysComponent KeysComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            InputReceiverComponent = Entity.Get<InputReceiverComponent>();
            KeysComponent = Entity.Get<KeysComponent>();
        }
    }
}
