using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class SerializableNode : NodeBase
    {
        public SerializableComponent SerializableComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            SerializableComponent = Entity.Get<SerializableComponent>();
        }
    }
}
