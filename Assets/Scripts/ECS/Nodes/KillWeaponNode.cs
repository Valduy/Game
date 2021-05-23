using System;
using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.ECS.Nodes
{
    public class KillWeaponNode : NodeBase
    {
        public OwnerHealthComponentComponent OwnerHealthComponentComponent { get; private set; }
        public AttackEnableComponent AttackEnableComponent { get; private set; }

        protected override void OnEntityChanged()
        {
            OwnerHealthComponentComponent = Entity.Get<OwnerHealthComponentComponent>();
            AttackEnableComponent = Entity.Get<AttackEnableComponent>();
        }
    }
}
