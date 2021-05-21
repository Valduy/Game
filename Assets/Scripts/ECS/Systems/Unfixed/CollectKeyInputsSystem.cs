using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Unfixed
{
    public class CollectKeyInputsSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<KeysNode>())
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
