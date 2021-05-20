using System;
using ECS.Core;
using ECS.Serialization.Attributes;
using ECS.Serialization.Converters;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    [ComponentConverter(typeof(PositionComponentConverter))]
    public class PositionComponent : ComponentBase
    {
        public Vector3 Position;
    }

    public class PositionComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var positionComponent = (PositionComponent)component;
            writer.WriteFloat((float)Math.Round(positionComponent.Position.x, 2, MidpointRounding.AwayFromZero));
            writer.WriteFloat((float)Math.Round(positionComponent.Position.y, 2, MidpointRounding.AwayFromZero));
            writer.WriteFloat((float)Math.Round(positionComponent.Position.z, 2, MidpointRounding.AwayFromZero));
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader)
            => new PositionComponent { Position = { x = reader.ReadFloat(), y = reader.ReadFloat(), z = reader.ReadFloat() } };
    }
}
