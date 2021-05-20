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
                    -BoolToInt(node.KeyComponent.A) + BoolToInt(node.KeyComponent.D),
                    -BoolToInt(node.KeyComponent.S) + BoolToInt(node.KeyComponent.W));
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private static int BoolToInt(bool b) => b ? 1 : 0;
    }
}
