using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Unfixed
{
    class CollectMouseInputsSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<MouseNode>())
            {
                var screenMousePosition = Input.mousePosition;
                var camera = node.CameraComponent.Camera;
                node.MouseComponent.MousePosition = camera.ScreenToWorldPoint(new Vector3(
                    screenMousePosition.x,
                    screenMousePosition.y,
                    node.CameraComponent.Camera.nearClipPlane));
            }
        }

        public override void Register(Engine engine)
        {
            _engine = engine;
        }

        public override void Unregister(Engine engine)
        {
            _engine = null;
        }
    }
}
