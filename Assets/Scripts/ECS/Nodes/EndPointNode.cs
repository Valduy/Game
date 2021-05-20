using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class EndPointNode : NodeBase
    {
        public EndPointComponent EndPointComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            EndPointComponent = Entity.Get<EndPointComponent>();
        }
    }
}
