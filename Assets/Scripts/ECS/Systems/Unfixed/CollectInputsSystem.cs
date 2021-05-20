using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Unfixed
{
    public class CollectInputsSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<KeyNode>())
            {
                node.KeyComponent.W |= Input.GetKey(KeyCode.W);
                node.KeyComponent.A |= Input.GetKey(KeyCode.A);
                node.KeyComponent.S |= Input.GetKey(KeyCode.S);
                node.KeyComponent.D |= Input.GetKey(KeyCode.D);
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
