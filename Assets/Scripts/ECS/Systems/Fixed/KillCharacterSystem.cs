using System.Linq;
using Assets.Scripts.ECS.Components;
using ECS.Core;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class KillCharacterSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public HealthComponent HealthComponent { get; private set; }
            public MoveEnableComponent MoveEnableComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                HealthComponent = Entity.Get<HealthComponent>();
                MoveEnableComponent = Entity.Get<MoveEnableComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                if (node.HealthComponent.CurrentHealth <= 0)
                {
                    node.Entity.Remove<MoveEnableComponent>();
                    Debug.Log("kill");
                }
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
