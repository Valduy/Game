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
    class AttackTimeoutSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public ShortAttackTimeoutComponent ShortAttackTimeoutComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                ShortAttackTimeoutComponent = Entity.Get<ShortAttackTimeoutComponent>();
            }
        }

        public class NodeWide : NodeBase
        {
            public WideAttackTimeoutComponent WideAttackTimeoutComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                WideAttackTimeoutComponent = Entity.Get<WideAttackTimeoutComponent>();
            }
        }

        Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                node.ShortAttackTimeoutComponent.Timeout += (float)time;
                var timeoutTime = node.ShortAttackTimeoutComponent.Timeout;
                Debug.Log(timeoutTime);
                

                if (timeoutTime < .3f) continue;
                
                node.Entity.Remove<ShortAttackTimeoutComponent>();
            }

            foreach (var node in _engine.GetNodes<NodeWide>().ToList())
            {
                node.WideAttackTimeoutComponent.Timeout += (float)time;
                var timeoutTime = node.WideAttackTimeoutComponent.Timeout;

                if (timeoutTime < 1.0f) continue;

                node.Entity.Remove<WideAttackTimeoutComponent>();
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
