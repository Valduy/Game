using System;
using Assets.Scripts.ECS.Nodes;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class ResetInputsSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<KeyNode>())
            {
                node.KeyComponent.W = false;
                node.KeyComponent.A = false;
                node.KeyComponent.S = false;
                node.KeyComponent.D = false;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
