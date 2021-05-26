using System.Linq;
using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Nodes;
using ECS.Core;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class KillCharacterSystem : SystemBase
    {
        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<KillCharacterNode>().ToList())
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
