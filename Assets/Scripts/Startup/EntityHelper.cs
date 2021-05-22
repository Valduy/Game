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
                .Add(new PositionComponent())
                .Id(id);

        public static Entity GetClientPlayerEntity(GameObject player, uint id, float speed)
            => GetPlayerEntityBase(player, speed)
                .Add(new PositionComponent())
                .Id(id);

        private static Entity GetPlayerEntityBase(GameObject player, float speed)
            => new Entity()
                .Add(new VelocityComponent())
                .Add(new TransformComponent { Transform = player.GetComponent<Transform>() })
                .Add(new RigidbodyComponent { Rigidbody = player.GetComponent<Rigidbody2D>() });

        #endregion

        #region Sword.

        public static Entity GetWeaponEntity(GameObject sword, GameObject owner, float r) 
            => GetWeaponEntityBase(sword)
                .Add(new OwnerTransformComponent { Transform = owner.GetComponent<Transform>() })
                .Add(new WeaponRadiusComponent { R = r });

        public static Entity GetHostWeaponEntity(GameObject sword, GameObject owner, uint id, float r)
            => GetWeaponEntity(sword, owner, r)
                .Add(new PositionComponent())
                .Add(new RotationComponent())
                .Id(id);

        public static Entity GetClientWeaponEntity(GameObject sword, uint id) 
            => GetWeaponEntityBase(sword)
                .Add(new PositionComponent())
                .Add(new RotationComponent())
                .Id(id);

        private static Entity GetWeaponEntityBase(GameObject sword)
            => new Entity()
                .Add(new TransformComponent {Transform = sword.GetComponent<Transform>()});

        #endregion

        #region Boss.

        // TODO: возможно, удастся без проблем унифицировать с игровыми персонажами.
        public static Entity GetBossEntity(GameObject boss, float speed) 
            => GetPlayerEntity(boss, speed);

        #endregion

        #region Camera.

        public static Entity GetCameraEntity(GameObject camera, GameObject target)
            => new Entity()
                .Add(new FollowComponent())
                .Add(new TransformComponent { Transform = camera.GetComponent<Transform>() })
                .Add(new OwnerTransformComponent { Transform = target.GetComponent<Transform>() });

        #endregion

        public static Entity KeyInputSource(this Entity entity)
            => entity.Add(new KeysComponent());

        public static Entity KeyInputsReceiver(this Entity entity) 
            => entity.KeyInputSource()
                .Add(new InputReceiverComponent());

        public static Entity MouseInputSource(this Entity entity) 
            => entity.Add(new MouseComponent());

        public static Entity MouseInputReceiver(this Entity entity, GameObject camera)
            => entity.MouseInputSource()
                .Add(new InputReceiverComponent())
                .Add(new CameraComponent {Camera = camera.GetComponent<Camera>()});

        public static Entity Serializable(this Entity entity) 
            => entity.Add(new SerializableComponent());

        public static Entity Id(this Entity entity, uint id) 
            => entity.Add(new IdComponent {Id = id});
    }
}
