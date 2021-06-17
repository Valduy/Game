using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using ECS.Core;
using Assets.Scripts.ECS.Systems.Fixed;

namespace Tests
{
    public class GoalFindTest
    {
        [Test]
        public void GoalFindTestSimplePasses()
        {
            var _goEnemy = GameObject.Instantiate(new GameObject(), Vector2.zero, Quaternion.identity);
            var _goPlayer1 = GameObject.Instantiate(new GameObject(), new Vector2(0, 35), Quaternion.identity);
            var _goPlayer2 = GameObject.Instantiate(new GameObject(), new Vector2(0, 15), Quaternion.identity);

            Engine _engine = new Engine();

            _engine.AddSystem(new GoalFindSystem(), 0);

            TransformComponent transformComponent = new TransformComponent() { Transform = _goEnemy.transform };
            TransformComponent transformComponentP1 = new TransformComponent() { Transform = _goPlayer1.transform };
            TransformComponent transformComponentP2 = new TransformComponent() { Transform = _goPlayer2.transform };

            Entity _player1 = new Entity().Add(transformComponentP1);
            Entity _player2 = new Entity().Add(transformComponentP2);

            Entity _entity = new Entity()
                .Add(new SearchAvailableComponent())
                .Add(transformComponent)
                .Add(new GoalsAvailableComponent() { Goals = new Entity[] { _player1, _player2} });

            _engine.AddEntity(_entity);
            _engine.AddEntity(_player1);
            _engine.AddEntity(_player2);

            _engine.Update(0.1);

            Assert.AreEqual(1, _entity.Get<GoalComponent>().Goal);
        }
    }
}
