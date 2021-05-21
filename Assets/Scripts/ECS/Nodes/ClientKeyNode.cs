using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class ClientKeyNode : NodeBase
    {
        public KeysComponent KeysComponent { get; private set; }
        public EndPointComponent EndPointComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            KeysComponent = Entity.Get<KeysComponent>();
            EndPointComponent = Entity.Get<EndPointComponent>();
        }
    }
}
