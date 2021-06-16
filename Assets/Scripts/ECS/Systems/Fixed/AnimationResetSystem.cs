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
    class AnimationResetSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public AnimatorComponent AnimatorComponent { get; private set; }
            public DirectionComponent DirectionComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                AnimatorComponent = Entity.Get<AnimatorComponent>();
                DirectionComponent = Entity.Get<DirectionComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            MoveAnimation();
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private void MoveAnimation()
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                node.AnimatorComponent.Animator.SetBool("IsUpMove", false);
                node.AnimatorComponent.Animator.SetBool("IsDownMove", false);
                node.AnimatorComponent.Animator.SetBool("IsLeftMove", false);
                node.AnimatorComponent.Animator.SetBool("IsRightMove", false);
            }
        }
    }
}
