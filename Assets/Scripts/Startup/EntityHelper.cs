using System.Net;
using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public static class EntityHelper
    {
        public static Entity GetPlayerEntity(GameObject go, float speed)
            => GetPlayerBase(go, speed)
                .Add(new KeyComponent())
                .Add(new InputReceiverComponent());

        public static Entity GetThisPlayerEntity(GameObject go, uint id, float speed)
            => GetPlayerEntity(go, speed)
                .Add(new SerializableComponent())
                .Add(new PositionComponent())
                .Add(new IdComponent {Id = id});

        public static Entity GetOtherPlayerEntity(GameObject go, uint id, float speed, IPEndPoint endPoint) 
            => GetPlayerBase(go, speed)
                .Add(new KeyComponent())
                .Add(new EndPointComponent { EndPoint = endPoint })
                .Add(new SerializableComponent())
                .Add(new PositionComponent())
                .Add(new IdComponent { Id = id });

        public static Entity GetThisPlayerPhantomEntity(GameObject go, uint id, float speed)
            => GetPlayerBase(go, speed)
                .Add(new KeyComponent())
                .Add(new InputReceiverComponent())
                .Add(new IdComponent {Id = 1})
                .Add(new PositionComponent());

        public static Entity GetOtherPlayerPhantomEntity(GameObject go, uint id, float speed) 
            => GetPlayerBase(go, speed)
                .Add(new IdComponent {Id = id})
                .Add(new PositionComponent());

        private static Entity GetPlayerBase(GameObject go, float speed) 
            => new Entity()
                .Add(new DirectionComponent())
                .Add(new SpeedComponent { Speed = speed })
                .Add(new TransformComponent { Transform = go.GetComponent<Transform>() })
                .Add(new RigidbodyComponent { Rigidbody = go.GetComponent<Rigidbody2D>() });
    }
}
