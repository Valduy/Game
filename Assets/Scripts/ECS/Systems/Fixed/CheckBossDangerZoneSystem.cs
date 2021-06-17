using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ECS.Core;
using Assets.Scripts.ECS.Components;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class CheckBossDangerZoneSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public TransformComponent TransformComponent { get; private set; }
            public DangerZoneComponent DangerZoneComponent { get; private set; }
            public ItsWeaponEntityComponent ItsWeaponEntityComponent { get; private set; }
            public GoalComponent GoalComponent { get;  private set; }
            public GoalsAvailableComponent GoalsAvailableComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                TransformComponent = Entity.Get<TransformComponent>();
                DangerZoneComponent = Entity.Get<DangerZoneComponent>();
                ItsWeaponEntityComponent = Entity.Get<ItsWeaponEntityComponent>();
                GoalComponent = Entity.Get<GoalComponent>();
                GoalsAvailableComponent = Entity.Get<GoalsAvailableComponent>();
            }
        }


        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                if (node.ItsWeaponEntityComponent.Weapon.Contain<VirtualMouseGoalComponent>()) continue;

                var colliders = Physics2D.OverlapCircleAll(node.TransformComponent.Transform.position,
                    node.DangerZoneComponent.Radius, LayerMask.GetMask("Characters"));

                if (colliders.Length == 1) continue;

                var goal = colliders.Where(
                    el => el.gameObject.name == node.GoalsAvailableComponent.Goals[node.GoalComponent.Goal]
                    .Get<ColliderComponent>().Collider.gameObject.name).ToList();
                if (goal.Count == 0)
                {
                    SetFindActive(node);
                    continue;
                }

                var angle = GetAngleBetween(goal[0].gameObject.transform, node.TransformComponent.Transform);

                node.ItsWeaponEntityComponent.Weapon.Add(new VirtualMouseGoalComponent() { Tendention = 1, Angle = angle });
                node.ItsWeaponEntityComponent.Weapon.Add(new IsPrepareStateComponent());

                if (colliders.Length != 2)
                    node.ItsWeaponEntityComponent.Weapon.Add(new IsWideAttackComponent() { });
                else
                    node.ItsWeaponEntityComponent.Weapon.Add(new IsShortAttackComponent() { });
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private float GetAngleBetween(Transform go1, Transform go2)
        {
            var angle = Mathf.Atan2(go1.position.y - go2.position.y,
                    go1.position.x - go2.position.x) * 180 / Mathf.PI;
            if (angle < 0) angle += 360;

            return angle;
        }

        private void SetFindActive(Node node)
        {
            node.Entity.Remove<GoalComponent>();
            node.Entity.Add(new SearchAvailableComponent());
        }
    
        private void CheckDangerZone(Node node)
        {

        }
    }
}
