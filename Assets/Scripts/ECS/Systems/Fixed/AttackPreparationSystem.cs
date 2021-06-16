using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Core;
using Assets.Scripts.ECS.Components;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class AttackPreparationSystem: SystemBase
    {
        public class Node : NodeBase
        {
            public VirtualMouseComponent VirtualMouseComponent { get; private set; }
            public VirtualMouseGoalComponent VirtualMouseGoalComponent { get; private set; }
            public WeaponSpeedComponent WeaponSpeedComponent { get; private set; }
            public IsEnemyWeaponComponent IsEnemyWeaponComponent { get; private set; }
            public VirtualMouseAttackAngleComponent VirtualMouseAttackAngleComponent { get; private set; }
            public IsPrepareStateComponent IsPrepareStateComponent { get; private set; }
            public IsShortAttackComponent IsShortAttack { get; private set; }

            protected override void OnEntityChanged()
            {
                VirtualMouseComponent = Entity.Get<VirtualMouseComponent>();
                VirtualMouseGoalComponent = Entity.Get<VirtualMouseGoalComponent>();
                WeaponSpeedComponent = Entity.Get<WeaponSpeedComponent>();
                IsEnemyWeaponComponent = Entity.Get<IsEnemyWeaponComponent>();
                IsEnemyWeaponComponent = Entity.Get<IsEnemyWeaponComponent>();
                VirtualMouseAttackAngleComponent = Entity.Get<VirtualMouseAttackAngleComponent>();
                IsPrepareStateComponent = Entity.Get<IsPrepareStateComponent>();
                IsShortAttack = Entity.Get<IsShortAttackComponent>();
            }
        }

        public class NodeWide : NodeBase
        {
            public VirtualMouseComponent VirtualMouseComponent { get; private set; }
            public VirtualMouseGoalComponent VirtualMouseGoalComponent { get; private set; }
            public WeaponSpeedComponent WeaponSpeedComponent { get; private set; }
            public IsEnemyWeaponComponent IsEnemyWeaponComponent { get; private set; }
            public IsPrepareStateComponent IsPrepareStateComponent { get; private set; }
            public IsWideAttackComponent IsWideAttackComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                VirtualMouseComponent = Entity.Get<VirtualMouseComponent>();
                VirtualMouseGoalComponent = Entity.Get<VirtualMouseGoalComponent>();
                WeaponSpeedComponent = Entity.Get<WeaponSpeedComponent>();
                IsEnemyWeaponComponent = Entity.Get<IsEnemyWeaponComponent>();
                IsPrepareStateComponent = Entity.Get<IsPrepareStateComponent>();
                IsWideAttackComponent = Entity.Get<IsWideAttackComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            ShortAttackPrepare();
            WideAttackPrepare();
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private float GetNextAngle(float current, float addition)
        {
            var nextAngle = current + addition;

            if (nextAngle > 360) nextAngle -= 360;
            else if (nextAngle < 0) nextAngle = 360 + nextAngle;

            return nextAngle;
        }

        private void Prepare(float goalAngle, float speed)
        {

        }

        private void ShortAttackPrepare()
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                node.Entity.Add(new OldWeaponSpeedComponent() { Speed = node.WeaponSpeedComponent.Speed });
            }
        }

        private void WideAttackPrepare()
        {
            foreach (var node in _engine.GetNodes<NodeWide>().ToList())
            {
                node.Entity.Add(new OldWeaponSpeedComponent() { Speed = node.WeaponSpeedComponent.Speed });
            }
        }


    }
}
