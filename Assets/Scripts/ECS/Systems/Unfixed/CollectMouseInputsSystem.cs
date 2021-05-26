using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Unfixed
{
    class CollectMouseInputsSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public IsInputReceiverComponent IsInputReceiverComponent { get; private set; }
            public MouseComponent MouseComponent { get; private set; }
            public CameraComponent CameraComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                MouseComponent = Entity.Get<MouseComponent>();
                IsInputReceiverComponent = Entity.Get<IsInputReceiverComponent>();
                CameraComponent = Entity.Get<CameraComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                var screenMousePosition = Input.mousePosition;
                var camera = node.CameraComponent.Camera;
                node.MouseComponent.MousePosition = camera.ScreenToWorldPoint(new Vector3(
                    screenMousePosition.x,
                    screenMousePosition.y,
                    node.CameraComponent.Camera.nearClipPlane));
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
