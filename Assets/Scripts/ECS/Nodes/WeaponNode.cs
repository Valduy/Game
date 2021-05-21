using System;
using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    class WeaponNode : NodeBase
    {
        public KeyComponent KeyComponent { get; private set; }
        public TransformComponent TransformComponent { get; private set; }
        public OwnerTransformComponent OwnerTransformComponent { get; private set; }
        public CameraComponent CameraComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            KeyComponent = Entity.Get<KeyComponent>();
            TransformComponent = Entity.Get<TransformComponent>();
            OwnerTransformComponent = Entity.Get<OwnerTransformComponent>();
            CameraComponent = Entity.Get<CameraComponent>();
        }
    }
}
