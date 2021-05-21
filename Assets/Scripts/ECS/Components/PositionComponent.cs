using Assets.Scripts.Networking.Serializers;
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
            ConverterHelper.WriteVector3(positionComponent.Position, 2, writer);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader)
            => new PositionComponent { Position = ConverterHelper.ReadVector3(reader) };
    }
}
