﻿using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class MoveNode : NodeBase
    {
        public VelocityComponent VelocityComponent { get; private set; }
        public RigidbodyComponent RigidbodyComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            VelocityComponent = Entity.Get<VelocityComponent>();
            RigidbodyComponent = Entity.Get<RigidbodyComponent>();
        }
    }
}
