using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using ECS.Core;
using Assets.Scripts.ECS.Systems.Fixed;

namespace Tests
{
    public class DirectionTest
    {
        [Test]
        public void CheckUpDirection()
        {
            Engine _engine = new Engine();

            KeysComponent keysComponent = new KeysComponent() { W = true };
            DirectionComponent directionComponent = new DirectionComponent();
            IsMoveEnableComponent isMoveEnableComponent = new IsMoveEnableComponent();

            Entity _entity = new Entity()
                .Add(keysComponent)
                .Add(directionComponent)
                .Add(isMoveEnableComponent);


            _engine.AddEntity(_entity);

            _engine.AddSystem(new CalculateDirectionSystem(), 0);

            _engine.Update(0.1);

            Assert.AreEqual(new Vector2(0, 1), directionComponent.Direction);
        }

        [Test]
        public void CheckDownDirection()
        {
            Engine _engine = new Engine();

            KeysComponent keysComponent = new KeysComponent() { S = true };
            DirectionComponent directionComponent = new DirectionComponent();
            IsMoveEnableComponent isMoveEnableComponent = new IsMoveEnableComponent();

            Entity _entity = new Entity()
                .Add(keysComponent)
                .Add(directionComponent)
                .Add(isMoveEnableComponent);


            _engine.AddEntity(_entity);

            _engine.AddSystem(new CalculateDirectionSystem(), 0);

            _engine.Update(0.1);

            Assert.AreEqual(new Vector2(0, -1), directionComponent.Direction);
        }


        [Test]
        public void CheckLeftDirection()
        {
            Engine _engine = new Engine();

            KeysComponent keysComponent = new KeysComponent() { A = true };
            DirectionComponent directionComponent = new DirectionComponent();
            IsMoveEnableComponent isMoveEnableComponent = new IsMoveEnableComponent();

            Entity _entity = new Entity()
                .Add(keysComponent)
                .Add(directionComponent)
                .Add(isMoveEnableComponent);


            _engine.AddEntity(_entity);

            _engine.AddSystem(new CalculateDirectionSystem(), 0);

            _engine.Update(0.1);

            Assert.AreEqual(new Vector2(-1, 0), directionComponent.Direction);
        }


        [Test]
        public void CheckRightDirection()
        {
            Engine _engine = new Engine();

            KeysComponent keysComponent = new KeysComponent() { D = true };
            DirectionComponent directionComponent = new DirectionComponent();
            IsMoveEnableComponent isMoveEnableComponent = new IsMoveEnableComponent();

            Entity _entity = new Entity()
                .Add(keysComponent)
                .Add(directionComponent)
                .Add(isMoveEnableComponent);


            _engine.AddEntity(_entity);

            _engine.AddSystem(new CalculateDirectionSystem(), 0);

            _engine.Update(0.1);

            Assert.AreEqual(new Vector2(1, 0), directionComponent.Direction);
        }
    }
}
