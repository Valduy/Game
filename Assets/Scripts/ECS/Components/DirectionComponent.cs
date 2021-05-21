using Assets.Scripts.Networking.Serializers;
using ECS.Core;
using ECS.Serialization.Attributes;
using ECS.Serialization.Converters;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    [ComponentConverter(typeof(DirectionComponentConverter))]
    public class DirectionComponent : ComponentBase
    {
        public Vector2 Direction;
    }

    public class DirectionComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var directionComponent = (DirectionComponent) component;
            ConverterHelper.WriteVector2(directionComponent.Direction, writer);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader) 
            => new DirectionComponent {Direction = ConverterHelper.ReadVector2(reader)};
    }
}
