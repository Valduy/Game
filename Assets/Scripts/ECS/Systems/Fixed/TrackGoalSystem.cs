using System.Linq;
using UnityEngine;
using ECS.Core;
using Assets.Scripts.ECS.Components;


namespace Assets.Scripts.ECS.Systems.Fixed
{
    public class TrackGoalSystem : SystemBase
    {
        Engine _engine;

        public class Node: NodeBase
        {
            public TransformComponent TransformComponent { get; private set; }
            public GoalComponent GoalComponent { get; private set; }
            public DirectionComponent DirectionComponent { get; private set; }
            public GoalsAvailableComponent GoalsAvailableComponent { get; private set; }
            public IsMoveEnableComponent IsMoveEnableComponent { get; private set; }

            protected override void OnEntityChanged()
            {
                TransformComponent = Entity.Get<TransformComponent>();
                GoalComponent = Entity.Get<GoalComponent>();
                DirectionComponent = Entity.Get<DirectionComponent>();
                GoalsAvailableComponent = Entity.Get<GoalsAvailableComponent>();
                IsMoveEnableComponent = Entity.Get<IsMoveEnableComponent>();
            }
        }

        public override void Update(double time)
        {
            foreach (var node in _engine.GetNodes<Node>().ToList())
            {
                var direction = GetGoalDirection(
                    node.TransformComponent.Transform,
                    node.GoalsAvailableComponent.Goals[node.GoalComponent.Goal]
                    .Get<TransformComponent>().Transform);
                
                node.DirectionComponent.Direction = direction;
            }
        }

        public override void Register(Engine engine) => _engine = engine;

        public override void Unregister(Engine engine) => _engine = null;

        private Vector2 GetGoalDirection(Transform seeker, Transform goal)
        {
            var heading = goal.position - seeker.position;
            var distance = heading.magnitude;
            return heading / distance;
        }
    }
}
