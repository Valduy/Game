using System.Linq;
using UnityEngine;
using ECS.Core;
using Assets.Scripts.ECS.Components;

namespace Assets.Scripts.ECS.Systems.Fixed
{
    class GoalFindSystem : SystemBase
    {
        public class Node : NodeBase
        {
            public SearchAvailableComponent SearchAvailableComponent { get; private set; }
            public TransformComponent TransformComponent { get; private set; }
            public GoalsAvailableComponent GoalsAvailableComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                TransformComponent = Entity.Get<TransformComponent>();
                GoalsAvailableComponent = Entity.Get<GoalsAvailableComponent>();
                SearchAvailableComponent = Entity.Get<SearchAvailableComponent>();
            }
        }


        private Engine _engine;

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                int i = FindGoal(node.TransformComponent.Transform, node.GoalsAvailableComponent.Goals);
                node.Entity.Remove<SearchAvailableComponent>();

                if (i == -1) continue;

                node.Entity.Add(new GoalComponent() { Goal = i });
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private int FindGoal(Transform seeker, Entity[] goals)
        {
            float minDist = float.PositiveInfinity;
            float dist;

            int goalIndex = -1;
            for (int i = 0; i < goals.Length; i++)
            {
                dist = Vector3.Distance(seeker.position, goals[i].Get<TransformComponent>().Transform.position);

                if (dist < minDist)
                {
                    minDist = dist;
                    goalIndex = i;
                }
            }

            return goalIndex;
        }
    }
}
