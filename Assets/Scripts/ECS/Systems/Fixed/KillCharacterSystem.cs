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
            public IsMoveEnableComponent IsMoveEnableComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                HealthComponent = Entity.Get<HealthComponent>();
                IsMoveEnableComponent = Entity.Get<IsMoveEnableComponent>();
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
                    Debug.Log("kill");
                }
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
