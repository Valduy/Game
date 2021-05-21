using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class RotateWeaponSystem : SystemBase
    {
        private Engine _engine;
        private float R = 1.2f;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<WeaponNode>())
            {
                var cursorPosition = node.CameraComponent.Camera.ScreenToWorldPoint(new Vector3(
                        node.KeyComponent.MousePosition.x, 
                        node.KeyComponent.MousePosition.y,
                        node.CameraComponent.Camera.nearClipPlane));

                var relativeCursorPosition = cursorPosition - node.OwnerTransformComponent.Transform.position;
                Debug.Log(relativeCursorPosition);
                var direction = new Vector2(relativeCursorPosition.x, relativeCursorPosition.y).normalized;
                var pos = node.OwnerTransformComponent.Transform.position + new Vector3(direction.x, direction.y, 0) * R;
                node.TransformComponent.Transform.position = pos;
                node.TransformComponent.Transform.rotation = Quaternion.Euler(new Vector3(0, 0, GetRotationAngle(relativeCursorPosition)));


                //node.TransformComponent.Transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

                //node.TransformComponent.Transform.RotateAround(
                //    node.OwnerTransformComponent.Transform.position,
                //    Vector3.forward,
                //    5);
                //Debug.Log(cursorPosition);

                //GetRotationAngle(node.OwnerTransformComponent.Transform.position, direction);
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private float GetRotationAngle(Vector2 v)
        {
            var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle += 360;
            }

            //Debug.Log(direction);
            Debug.Log(angle);

            return angle;
        }
    }
}
