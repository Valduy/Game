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
    public class AvailableGoalsConfiguratorSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public GoalsAvailableComponent GoalsAvailableComponent{ get; private set; }
            public GoalComponent GoalComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                GoalsAvailableComponent = Entity.Get<GoalsAvailableComponent>();
                GoalComponent = Entity.Get<GoalComponent>();
            }
        }


        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                if (!node.GoalsAvailableComponent.Goals[node.GoalComponent.Goal].Contain<IsAliveComponent>())
                {
                    node.GoalsAvailableComponent.Goals =
                        node.GoalsAvailableComponent.Goals.Where((el, i) => i != node.GoalComponent.Goal).ToArray();
                    SetFindActive(node);
                    continue;
                }
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private void SetFindActive(Node node)
        {
            node.Entity.Remove<GoalComponent>();
            node.Entity.Add(new SearchAvailableComponent());
        }
    }
}
