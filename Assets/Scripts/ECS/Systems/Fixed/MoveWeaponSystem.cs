using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

using System.Linq;


namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class MoveWeaponSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public MouseComponent MouseComponent { get; private set; }
            public TransformComponent TransformComponent { get; private set; }
            public OwnerTransformComponent OwnerTransformComponent { get; private set; }
            public WeaponRadiusComponent WeaponRadiusComponent { get; private set; }
            public IsAttackEnableComponent IsAttackEnableComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                MouseComponent = Entity.Get<MouseComponent>();
                TransformComponent = Entity.Get<TransformComponent>();
                OwnerTransformComponent = Entity.Get<OwnerTransformComponent>();
                WeaponRadiusComponent = Entity.Get<WeaponRadiusComponent>();
                IsAttackEnableComponent = Entity.Get<IsAttackEnableComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>())
            {
                var relativeToOwner = node.MouseComponent.MousePosition - node.OwnerTransformComponent.Transform.position;
                var direction = new Vector2(relativeToOwner.x, relativeToOwner.y).normalized;
                var r = node.WeaponRadiusComponent.R;
                node.TransformComponent.Transform.localPosition = new Vector3(direction.x, direction.y, 0) * r;
                node.TransformComponent.Transform.rotation = Quaternion.Euler(new Vector3(0, 0, GetRotationAngle(relativeToOwner)));
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private float GetRotationAngle(Vector2 v)
        {
            var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }
    }
}
