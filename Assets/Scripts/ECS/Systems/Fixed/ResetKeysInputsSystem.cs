using Assets.Scripts.ECS.Nodes;
using ECS.Core;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class ResetKeysInputsSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<KeysNode>())
            {
                node.KeysComponent.W = false;
                node.KeysComponent.A = false;
                node.KeysComponent.S = false;
                node.KeysComponent.D = false;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
