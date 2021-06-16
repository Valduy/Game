using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS.Core;
using UnityEngine;

using Assets.Scripts.ECS.Components;
using Assets.Scripts.ECS.Nodes;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class EndGameSystem : SystemBase
    {
        public class NodeCharacter : NodeBase
        {
            public HealthComponent HealthComponent { get; private set; }
            public PlayerFlagComponent PlayerFlagComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                HealthComponent = Entity.Get<HealthComponent>();
                PlayerFlagComponent = Entity.Get<PlayerFlagComponent>();
            }
        }

        public class NodeEnemies : NodeBase
        {
            public HealthComponent HealthComponent { get; private set; }
            public EnemyFlagComponent EnemyFlagComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                HealthComponent = Entity.Get<HealthComponent>();
                EnemyFlagComponent = Entity.Get<EnemyFlagComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            if (_engine.GetNodes<NodeEnemies>().ToList().All(enemy => enemy.HealthComponent.CurrentHealth <= 0))
            {
                foreach (var node in _engine.GetNodes<MenuNode>().ToList())
                {
                    node.EndGameComponent.Win.SetActive(true);
                }
            }
            else if (_engine.GetNodes<NodeCharacter>().ToList().All(player => player.HealthComponent.CurrentHealth <= 0))
            {
                foreach (var node in _engine.GetNodes<MenuNode>().ToList())
                {
                    node.EndGameComponent.GameOver.SetActive(true);
                }
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;
    }
}
