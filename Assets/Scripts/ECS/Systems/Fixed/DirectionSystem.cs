using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class DirectionSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<DirectionNode>())
            {
                node.DirectionComponent.Direction = new Vector2(
                    -BoolToInt(node.KeysComponent.A) + BoolToInt(node.KeysComponent.D),
                    -BoolToInt(node.KeysComponent.S) + BoolToInt(node.KeysComponent.W));
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private static int BoolToInt(bool b) => b ? 1 : 0;
    }
}
