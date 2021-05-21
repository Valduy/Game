using System.Net;
using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public static class EntityHelper
    {
        public static Entity GetPlayerEntity(GameObject player, float speed)
            => GetPlayerBase(player, speed)
                .Add(new KeyComponent())
                .Add(new InputReceiverComponent());

        public static Entity GetThisPlayerEntity(GameObject player, uint id, float speed)
            => GetPlayerEntity(player, speed)
                .Add(new SerializableComponent())
                .Add(new PositionComponent())
                .Add(new IdComponent {Id = id});

        public static Entity GetOtherPlayerEntity(GameObject player, uint id, float speed, IPEndPoint endPoint) 
            => GetPlayerBase(player, speed)
                .Add(new KeyComponent())
                .Add(new EndPointComponent { EndPoint = endPoint })
                .Add(new SerializableComponent())
                .Add(new PositionComponent())
                .Add(new IdComponent { Id = id });

        public static Entity GetThisPlayerPhantomEntity(GameObject player, uint id, float speed)
            => GetPlayerBase(player, speed)
                .Add(new KeyComponent())
                .Add(new InputReceiverComponent())
                .Add(new IdComponent {Id = 1})
                .Add(new PositionComponent());

        public static Entity GetOtherPlayerPhantomEntity(GameObject player, uint id, float speed) 
            => GetPlayerBase(player, speed)
                .Add(new IdComponent {Id = id})
                .Add(new PositionComponent());

        private static Entity GetPlayerBase(GameObject player, float speed) 
            => new Entity()
                .Add(new DirectionComponent())
                .Add(new SpeedComponent { Speed = speed })
                .Add(new TransformComponent { Transform = player.GetComponent<Transform>() })
                .Add(new RigidbodyComponent { Rigidbody = player.GetComponent<Rigidbody2D>() });

        public static Entity GetSword(GameObject sword, GameObject owner, GameObject camera) 
            => new Entity()
                .Add(new KeyComponent())
                .Add(new InputReceiverComponent())
                .Add(new TransformComponent {Transform = sword.GetComponent<Transform>()})
                .Add(new OwnerTransformComponent {Transform = owner.GetComponent<Transform>()})
                .Add(new CameraComponent {Camera = camera.GetComponent<Camera>()});
    }
}
