using System.Net;
using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public static class EntityHelper
    {
        #region Player.

        public static Entity GetPlayerEntity(GameObject player, float speed)
            => GetPlayerEntityBase(player, speed)
                .Add(new DirectionComponent())
                .Add(new SpeedComponent { Speed = speed });

        public static Entity GetHostPlayerEntity(GameObject player, uint id, float speed)
            => GetPlayerEntity(player, speed)
                .Add(new SerializableComponent())
                .Add(new PositionComponent())
                .Add(new IdComponent { Id = id });

        public static Entity GetClientPlayerEntity(GameObject player, uint id, float speed)
            => GetPlayerEntityBase(player, speed)
                .Add(new IdComponent {Id = id})
                .Add(new PositionComponent());

        private static Entity GetPlayerEntityBase(GameObject player, float speed)
            => new Entity()
                .Add(new VelocityComponent())
                .Add(new TransformComponent { Transform = player.GetComponent<Transform>() })
                .Add(new RigidbodyComponent { Rigidbody = player.GetComponent<Rigidbody2D>() });

        #endregion

        #region Sword.

        public static Entity GetSwordEntity(GameObject sword, GameObject owner, float r) 
            => new Entity()
                .Add(new TransformComponent { Transform = sword.GetComponent<Transform>() })
                .Add(new OwnerTransformComponent { Transform = owner.GetComponent<Transform>() })
                .Add(new WeaponRadiusComponent { R = r });

        public static Entity GetHostSwordEntity(GameObject sword, GameObject owner, uint id, float r)
            => GetSwordEntity(sword, owner, r)
                .Add(new IdComponent { Id = id })
                .Add(new SerializableComponent())
                .Add(new PositionComponent())
                .Add(new RotationComponent());

        public static Entity GetClientSwordEntity(GameObject sword, uint id) 
            => new Entity()
                .Add(new IdComponent {Id = id})
                .Add(new TransformComponent { Transform = sword.GetComponent<Transform>() })
                .Add(new PositionComponent());

        #endregion

        #region Camera.

        public static Entity GetCameraEntity(GameObject camera, GameObject target)
            => new Entity()
                .Add(new FollowComponent())
                .Add(new TransformComponent { Transform = camera.GetComponent<Transform>() })
                .Add(new OwnerTransformComponent { Transform = target.GetComponent<Transform>() });

        #endregion

        public static Entity BindWithClient(this Entity entity, IPEndPoint endPoint)
            => entity.Add(new EndPointComponent {EndPoint = endPoint});

        public static Entity MakeEntityKeyInputSource(this Entity entity)
            => entity.Add(new KeysComponent());

        public static Entity MakeEntityKeyInputsReceiver(this Entity entity) 
            => entity.MakeEntityKeyInputSource()
                .Add(new InputReceiverComponent());

        public static Entity MakeEntityMouseInputSource(this Entity entity) 
            => entity.Add(new MouseComponent());

        public static Entity MakeEntityMouseInputReceiver(this Entity entity, GameObject camera)
            => entity.MakeEntityMouseInputSource()
                .Add(new InputReceiverComponent())
                .Add(new CameraComponent {Camera = camera.GetComponent<Camera>()});
    }
}
