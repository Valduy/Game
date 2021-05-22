using Assets.Scripts.ECS.Components;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public static class EntityHelper
    {
        #region Player.

        public static Entity GetCharacterEntity(GameObject character, float speed)
            => GetCharacterEntityBase(character)
                .Add(new DirectionComponent())
                .Add(new SpeedComponent { Speed = speed });

        public static Entity GetHostCharacterEntity(GameObject character, uint id, float speed)
            => GetCharacterEntity(character, speed)
                .Add(new PositionComponent())
                .Id(id);

        public static Entity GetClientCharacterEntity(GameObject character, uint id)
            => GetCharacterEntityBase(character)
                .Add(new PositionComponent())
                .Id(id);

        private static Entity GetCharacterEntityBase(GameObject character)
            => new Entity()
                .Add(new VelocityComponent())
                .Add(new TransformComponent { Transform = character.GetComponent<Transform>() })
                .Add(new RigidbodyComponent { Rigidbody = character.GetComponent<Rigidbody2D>() });

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
