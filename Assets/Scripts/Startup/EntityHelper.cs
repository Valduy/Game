using Assets.Scripts.ECS.Components;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.Startup
{
    public static class EntityHelper
    {
        #region Player.

        public static Entity GetCharacterEntity(GameObject characterGo, int health, float speed)
            => GetCharacterEntityBase(characterGo, health)
                .Add(new IsMoveEnableComponent())
                .Add(new DirectionComponent())                
                .Add(new ColliderComponent {Collider = characterGo.GetComponent<Collider2D>()})
                .Add(new SpeedComponent {Speed = speed});

        public static Entity GetHostCharacterEntity(GameObject characterGo, uint id, int health, float speed)
            => GetCharacterEntity(characterGo, health, speed)
                .Add(new PositionComponent())
                .Id(id);

        public static Entity GetClientCharacterEntity(GameObject characterGo, int health, uint id)
            => GetCharacterEntityBase(characterGo, health)
                .Add(new PositionComponent())
                .Id(id);

        private static Entity GetCharacterEntityBase(GameObject characterGo, int health)
            => new Entity()
                .Add(new VelocityComponent())
                .Add(new HealthComponent { MaxHealth = health, CurrentHealth = health })
                .Add(new TransformComponent {Transform = characterGo.GetComponent<Transform>()})
                .Add(new RigidbodyComponent {Rigidbody = characterGo.GetComponent<Rigidbody2D>()})
                .Add(new HealthBarComponent {HealthBar = UnityHelper.GetChildComponent<HealthBar>(characterGo)});

        #endregion

        #region Sword.

        public static Entity GetWeaponEntity(
            GameObject swordGo, 
            GameObject ownerGo, 
            Entity ownerEntity,
            float r, 
            int damage, 
            float delta) 
            => GetWeaponEntityBase(swordGo)
                .Add(new IsAttackEnableComponent())
                .Add(new WeaponPreviousAngleComponent())
                .Add(new OwnerTransformComponent {Transform = ownerGo.GetComponent<Transform>()})
                .Add(new OwnerHealthComponentComponent {HealthComponent = ownerEntity.Get<HealthComponent>()})
                .Add(new ColliderComponent {Collider = swordGo.GetComponent<Collider2D>()})
                .Add(new WeaponRadiusComponent { R = r })
                .Add(new WeaponEffectiveDeltaComponent {Delta = delta})
                .Add(new DamageComponent {Damage = damage});

        public static Entity GetHostWeaponEntity(
            GameObject swordGo, 
            GameObject ownerGo,
            Entity ownerEntity,
            uint id, 
            float r, 
            int damage, 
            float delta)
            => GetWeaponEntity(swordGo, ownerGo, ownerEntity, r, damage, delta)
                .Add(new PositionComponent())
                .Add(new RotationComponent())
                .Id(id);

        public static Entity GetClientWeaponEntity(GameObject swordGo, uint id) 
            => GetWeaponEntityBase(swordGo)
                .Add(new PositionComponent())
                .Add(new RotationComponent())
                .Id(id);

        private static Entity GetWeaponEntityBase(GameObject swordGo)
            => new Entity()
                .Add(new TransformComponent {Transform = swordGo.GetComponent<Transform>()});

        #endregion

        #region Camera.

        public static Entity GetCameraEntity(GameObject cameraGo, GameObject targetGo)
            => new Entity()
                .Add(new IsFollowComponent())
                .Add(new TransformComponent { Transform = cameraGo.GetComponent<Transform>() })
                .Add(new OwnerTransformComponent { Transform = targetGo.GetComponent<Transform>() });

        #endregion

        public static Entity KeyInputSource(this Entity entity)
            => entity.Add(new KeysComponent());

        public static Entity KeyInputsReceiver(this Entity entity) 
            => entity.KeyInputSource()
                .Add(new IsInputReceiverComponent());

        public static Entity MouseInputSource(this Entity entity) 
            => entity.Add(new MouseComponent());

        public static Entity MouseInputReceiver(this Entity entity, GameObject cameraGo)
            => entity.MouseInputSource()
                .Add(new IsInputReceiverComponent())
                .Add(new CameraComponent {Camera = cameraGo.GetComponent<Camera>()});

        public static Entity Serializable(this Entity entity) 
            => entity.Add(new IsSerializableComponent());

        public static Entity Id(this Entity entity, uint id) 
            => entity.Add(new IdComponent {Id = id});
    }
}
