using Assets.Scripts.Networking.Serializers;
using ECS.Core;
using ECS.Serialization.Attributes;
using ECS.Serialization.Converters;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    [ComponentConverter(typeof(RotationComponentConverter))]
    public class RotationComponent : ComponentBase
    {
        public Quaternion Rotation;
    }

    public class RotationComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var rotationComponent = (RotationComponent) component;
            ConverterHelper.WriteQuaternion(rotationComponent.Rotation, writer);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader) 
            => new RotationComponent {Rotation = ConverterHelper.ReadQuaternion(reader)};
    }
}
