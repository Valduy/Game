using Assets.Scripts.Networking.Serializers;
using ECS.Core;
using ECS.Serialization.Attributes;
using ECS.Serialization.Converters;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, содержащий скорость сущности.
    /// </summary>
    [ComponentConverter(typeof(VelocityComponentConverter))]
    public class VelocityComponent : ComponentBase
    {
        /// <summary>
        /// Скорость сущности.
        /// </summary>
        public Vector2 Velocity;
    }

    public class VelocityComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var directionComponent = (VelocityComponent)component;
            ConverterHelper.WriteVector2(directionComponent.Velocity, writer);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader)
            => new VelocityComponent { Velocity = ConverterHelper.ReadVector2(reader) };
    }
}
