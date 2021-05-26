using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Unfixed
{
    public class CollectKeyInputsSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public IsInputReceiverComponent IsInputReceiverComponent { get; private set; }
            public KeysComponent KeysComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                IsInputReceiverComponent = Entity.Get<IsInputReceiverComponent>();
                KeysComponent = Entity.Get<KeysComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                node.KeysComponent.W |= Input.GetKey(KeyCode.W);
                node.KeysComponent.A |= Input.GetKey(KeyCode.A);
                node.KeysComponent.S |= Input.GetKey(KeyCode.S);
                node.KeysComponent.D |= Input.GetKey(KeyCode.D);
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
