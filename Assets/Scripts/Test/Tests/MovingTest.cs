using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using ECS.Core;
using Assets.Scripts.ECS.Systems.Fixed;

namespace Tests
{
    public class MovingTest
    {
        [Test]
        public void MovingTestSimplePasses()
        {
            var _goEnemy = GameObject.Instantiate(new GameObject(), Vector2.zero, Quaternion.identity);
            _goEnemy.AddComponent<Rigidbody2D>();

            Engine _engine = new Engine();

            _engine.AddSystem(new MoveCharacterSystem(), 0);

            Entity _entity = new Entity()
                .Add(new VelocityComponent() { Velocity = new Vector2(0, 1) })
                .Add(new RigidbodyComponent() { Rigidbody = _goEnemy.GetComponent<Rigidbody2D>()});

            _engine.AddEntity(_entity);

            _engine.Update(0.1);

            Assert.AreEqual(_entity.Get<RigidbodyComponent>().Rigidbody.velocity, new Vector2(0, 1));
        }
    }
}
