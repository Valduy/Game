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
            public IsAliveComponent IsAliveComponent { get; private set; }
            public PlayerFlagComponent PlayerFlagComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                IsAliveComponent = Entity.Get<IsAliveComponent>();
                PlayerFlagComponent = Entity.Get<PlayerFlagComponent>();
            }
        }

        public class NodeEnemies : NodeBase
        {
            public IsAliveComponent IsAliveComponent { get; private set; }
            public EnemyFlagComponent EnemyFlagComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                IsAliveComponent = Entity.Get<IsAliveComponent>();
                EnemyFlagComponent = Entity.Get<EnemyFlagComponent>();
            }
        }

        private Engine _engine;

        public override void Update(double time)
        {
            if (_engine.GetNodes<NodeEnemies>().ToList().Count <= 0)
            {
                foreach (var node in _engine.GetNodes<MenuNode>().ToList())
                {
                    node.EndGameComponent.Win.SetActive(true);
                }
            }
            if (_engine.GetNodes<NodeCharacter>().ToList().Count <= 0)
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
