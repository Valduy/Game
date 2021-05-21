using System.Linq;
using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class MoveCameraSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<CameraNode>())
            {
                node.TransformComponent.Transform.position = new Vector3(
                    node.OwnerTransformComponent.Transform.position.x,
                    node.OwnerTransformComponent.Transform.position.y,
                    node.TransformComponent.Transform.position.z);
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
