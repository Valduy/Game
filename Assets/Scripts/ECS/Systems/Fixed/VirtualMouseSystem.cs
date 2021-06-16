using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Core;
using Assets.Scripts.ECS.Components;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class VirtualMouseSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public VirtualMouseComponent VirtualMouseComponent { get; private set; }
            public VirtualMouseGoalComponent VirtualMouseGoalComponent { get; private set; }
            public WeaponSpeedComponent WeaponSpeedComponent { get; private set; }
            public IsEnemyWeaponComponent IsEnemyWeaponComponent { get; private set; }
            public VirtualMouseAttackAngleComponent VirtualMouseAttackAngleComponent { get; private set; }
            public OldWeaponSpeedComponent OldWeaponSpeedComponent { get; private set; }
            public IsShortAttackComponent IsShortAttack { get; private set; }

            protected override void OnEntityChanged()
            {
                VirtualMouseComponent = Entity.Get<VirtualMouseComponent>();
                VirtualMouseGoalComponent = Entity.Get<VirtualMouseGoalComponent>();
                WeaponSpeedComponent = Entity.Get<WeaponSpeedComponent>();
                IsEnemyWeaponComponent = Entity.Get<IsEnemyWeaponComponent>();
                IsEnemyWeaponComponent = Entity.Get<IsEnemyWeaponComponent>();
                VirtualMouseAttackAngleComponent = Entity.Get<VirtualMouseAttackAngleComponent>();
                OldWeaponSpeedComponent = Entity.Get<OldWeaponSpeedComponent>();
                IsShortAttack = Entity.Get<IsShortAttackComponent>();
            }
        }

        public class NodeWide : NodeBase
        {
            public VirtualMouseComponent VirtualMouseComponent { get; private set; }
            public VirtualMouseGoalComponent VirtualMouseGoalComponent { get; private set; }
            public WeaponSpeedComponent WeaponSpeedComponent { get; private set; }
            public IsEnemyWeaponComponent IsEnemyWeaponComponent { get; private set; }
            public OldWeaponSpeedComponent OldWeaponSpeedComponent { get; private set; }
            public IsWideAttackComponent IsWideAttackComponent { get; private set; }


            protected override void OnEntityChanged()
            {
                VirtualMouseComponent = Entity.Get<VirtualMouseComponent>();
                VirtualMouseGoalComponent = Entity.Get<VirtualMouseGoalComponent>();
                WeaponSpeedComponent = Entity.Get<WeaponSpeedComponent>();
                IsEnemyWeaponComponent = Entity.Get<IsEnemyWeaponComponent>();
                OldWeaponSpeedComponent = Entity.Get<OldWeaponSpeedComponent>();
                IsWideAttackComponent = Entity.Get<IsWideAttackComponent>();
            }
        }

        Engine _engine;

        public override void Update(double time)
        {
            ShortAttack();
            WideAttack();
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

        private void ShortAttack()
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                var workingBaseAngle = node.VirtualMouseAttackAngleComponent.Angle;

                var goalAngle = node.VirtualMouseGoalComponent.Angle;

                var difference = workingBaseAngle / 2;
                var angle1 = GetNextAngle(goalAngle, -difference);
                var angle2 = GetNextAngle(goalAngle, difference);

                var currentAngle = node.VirtualMouseComponent.Angle;

                if (node.VirtualMouseGoalComponent.CurrentStage == 0)
                {
                    node.VirtualMouseComponent.Angle = angle1;
                    node.VirtualMouseGoalComponent.CurrentStage = 1;
                    continue;
                }

                if (currentAngle > angle2)
                {
                    node.WeaponSpeedComponent.Speed = node.OldWeaponSpeedComponent.Speed;
                    node.Entity.Remove<VirtualMouseGoalComponent>();
                    continue;
                }

                float ang = GetNextAngle(node.VirtualMouseComponent.Angle,
                    node.VirtualMouseGoalComponent.Tendention * node.WeaponSpeedComponent.Speed);
                
                node.WeaponSpeedComponent.Speed += 0.1f;

                node.VirtualMouseComponent.Angle = ang;
            }
        }

        private void WideAttack()
        {
            foreach (var node in _engine.GetNodes<NodeWide>().ToList())
            {
                var workingBaseAngle = 340;

                var goalAngle = node.VirtualMouseGoalComponent.Angle;

                var difference = workingBaseAngle / 2;
                var angle1 = GetNextAngle(goalAngle, -difference);
                var angle2 = GetNextAngle(goalAngle, difference);

                var currentAngle = node.VirtualMouseComponent.Angle;

                if (node.VirtualMouseGoalComponent.CurrentStage == 0)
                {
                    node.VirtualMouseComponent.Angle = angle1;

                    node.VirtualMouseGoalComponent.CurrentStage = 1;
                    continue;
                }

                if(angle1 < angle2)
                {
                    var t = angle1;
                    angle1 = angle2;
                    angle2 = t;
                }
                if (currentAngle > angle2 && currentAngle < angle1)
                {
                    node.WeaponSpeedComponent.Speed = node.OldWeaponSpeedComponent.Speed;

                    node.Entity.Remove<VirtualMouseGoalComponent>();
                    node.Entity.Remove<IsWideAttackComponent>();
                    continue;
                }
                float ang = GetNextAngle(node.VirtualMouseComponent.Angle,
                    node.VirtualMouseGoalComponent.Tendention * node.WeaponSpeedComponent.Speed);
                node.WeaponSpeedComponent.Speed += 0.3f;

                node.VirtualMouseComponent.Angle = ang;
            }
        }
    }
}
