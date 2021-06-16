using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Core;
using UnityEngine;

using Assets.Scripts.ECS.Components;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class AnimationSystem : SystemBase
    {
        public class NodeMovable : NodeBase
        {
            public AnimatorComponent AnimatorComponent { get; private set; }
            public DirectionComponent DirectionComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                AnimatorComponent = Entity.Get<AnimatorComponent>();
                DirectionComponent = Entity.Get<DirectionComponent>();
            }
        }

        public class NodeDeathable : NodeBase
        {
            public AnimatorComponent AnimatorComponent { get; private set; }
            public HealthComponent HealthComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                AnimatorComponent = Entity.Get<AnimatorComponent>();
                HealthComponent = Entity.Get<HealthComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            MoveAnimation();
            DeathAnimation();
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private void MoveAnimation()
        {
            foreach (var node in _engine.GetNodes<NodeMovable>().ToList())
            {
                node.AnimatorComponent.Animator.SetBool("IsUpMove", node.DirectionComponent.Direction.y > 0);
                node.AnimatorComponent.Animator.SetBool("IsDownMove", node.DirectionComponent.Direction.y < 0);

                node.AnimatorComponent.Animator.SetBool("IsRightMove", node.DirectionComponent.Direction.x > 0);
                node.AnimatorComponent.Animator.SetBool("IsLeftMove", node.DirectionComponent.Direction.x < 0);
            }
        }

        private void DeathAnimation()
        {
            foreach (var node in _engine.GetNodes<NodeDeathable>().ToList())
            {
                //Debug.Log($"{node.HealthComponent.CurrentHealth}");
                node.AnimatorComponent.Animator.SetBool("IsDeath", node.HealthComponent.CurrentHealth <= 0);
            }
        }
    }
}
