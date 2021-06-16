using System.Linq;
using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class KillCharacterSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public HealthComponent HealthComponent { get; private set; }
            public IsMoveEnableComponent IsMoveEnableComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                HealthComponent = Entity.Get<HealthComponent>();
                IsMoveEnableComponent = Entity.Get<IsMoveEnableComponent>();
            }
        }

        public class NodeDead : NodeBase
        {
            public IsDeadComponent IsDeadComponent { get; private set; }
            public DeathTimerComponent DeathTimerComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                IsDeadComponent = Entity.Get<IsDeadComponent>();
                DeathTimerComponent = Entity.Get<DeathTimerComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                if (node.HealthComponent.CurrentHealth <= 0)
                {
                    node.Entity.Remove<IsMoveEnableComponent>();
                    node.Entity.Remove<IsAliveComponent>();

                    node.Entity.Add(new IsDeadComponent());
                    node.Entity.Add(new DeathTimerComponent() { TimeLeft = 0 });
                    node.Entity.Get<ColliderComponent>().Collider.enabled = false;
                }
            }

            foreach (var node in _engine.GetNodes<NodeDead>().ToList())
            {
                node.DeathTimerComponent.TimeLeft += Time.deltaTime;

                if(node.DeathTimerComponent.TimeLeft > 3)
                {
                    node.Entity.Get<TransformComponent>().Transform.gameObject.SetActive(false);
                }
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
