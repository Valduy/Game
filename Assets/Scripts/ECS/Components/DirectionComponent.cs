using System;
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
            writer.WriteFloat(directionComponent.Direction.x);
            writer.WriteFloat(directionComponent.Direction.y);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader) 
            => new DirectionComponent {Direction = {x = reader.ReadFloat(), y = reader.ReadFloat()}};
    }
}
