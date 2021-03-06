using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class CalculateDirectionSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public DirectionComponent DirectionComponent { get; set; }
            public KeysComponent KeysComponent { get; set; }
            public IsMoveEnableComponent IsMoveEnableComponent { get; set; }

            protected override void OnEntityChanged()
            {
                DirectionComponent = Entity.Get<DirectionComponent>();
                KeysComponent = Entity.Get<KeysComponent>();
                IsMoveEnableComponent = Entity.Get<IsMoveEnableComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                node.DirectionComponent.Direction = new Vector2(
                    -BoolToInt(node.KeysComponent.A) + BoolToInt(node.KeysComponent.D),
                    -BoolToInt(node.KeysComponent.S) + BoolToInt(node.KeysComponent.W)).normalized;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private static int BoolToInt(bool b) => b ? 1 : 0;
    }
}
