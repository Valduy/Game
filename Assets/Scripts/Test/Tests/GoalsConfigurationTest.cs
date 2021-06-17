using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using ECS.Core;
using Assets.Scripts.ECS.Systems.Fixed;

namespace Tests
{
    public class GoalsConfigurationTest
    {
        [Test]
        public void OnUntouchGoal()
        {
            var _goEnemy = GameObject.Instantiate(new GameObject(), Vector2.zero, Quaternion.identity);
            var _goPlayer1 = GameObject.Instantiate(new GameObject(), new Vector2(0, 35), Quaternion.identity);

            Engine _engine = new Engine();

            _engine.AddSystem(new AvailableGoalsConfiguratorSystem(), 0);

            TransformComponent transformComponent = new TransformComponent() { Transform = _goEnemy.transform };
            TransformComponent transformComponentP1 = new TransformComponent() { Transform = _goPlayer1.transform };

            Entity _player1 = new Entity().Add(transformComponentP1);

            Entity _entity = new Entity()
                .Add(transformComponent)
                .Add(new GoalsAvailableComponent() { Goals = new Entity[] { _player1 } })
                .Add(new GoalComponent() { Goal = 0 });

            _engine.AddEntity(_entity);
            _engine.AddEntity(_player1);

            _engine.Update(0.1);

            Assert.AreEqual(false, _entity.Contain<GoalComponent>());
        }

        [Test]
        public void OnDeleteGoal()
        {
            var _goEnemy = GameObject.Instantiate(new GameObject(), Vector2.zero, Quaternion.identity);
            var _goPlayer1 = GameObject.Instantiate(new GameObject(), new Vector2(0, 35), Quaternion.identity);

            Engine _engine = new Engine();

            _engine.AddSystem(new AvailableGoalsConfiguratorSystem(), 0);

            TransformComponent transformComponent = new TransformComponent() { Transform = _goEnemy.transform };
            TransformComponent transformComponentP1 = new TransformComponent() { Transform = _goPlayer1.transform };

            Entity _player1 = new Entity().Add(transformComponentP1);

            Entity _entity = new Entity()
                .Add(transformComponent)
                .Add(new GoalsAvailableComponent() { Goals = new Entity[] { _player1 } })
                .Add(new GoalComponent() { Goal = 0 });

            _engine.AddEntity(_entity);
            _engine.AddEntity(_player1);

            _engine.Update(0.1);

            Assert.AreEqual(0, _entity.Get<GoalsAvailableComponent>().Goals.Length);
        }
    }
}
