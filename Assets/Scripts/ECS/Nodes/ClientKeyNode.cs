using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class ClientKeyNode : NodeBase
    {
        public KeyComponent KeyComponent { get; private set; }
        public EndPointComponent EndPointComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            KeyComponent = Entity.Get<KeyComponent>();
            EndPointComponent = Entity.Get<EndPointComponent>();
        }
    }
}
