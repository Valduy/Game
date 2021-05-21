using Assets.Scripts.Networking.Serializers;
using ECS.Core;
using ECS.Serialization.Attributes;
using ECS.Serialization.Converters;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    [ComponentConverter(typeof(KeyComponentConverter))]
    public class KeyComponent : ComponentBase
    {
        public bool W;
        public bool A;
        public bool S;
        public bool D;
        public Vector3 MousePosition;
    }

    public class KeyComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var keyComponent = (KeyComponent) component;
            writer.WriteBool(keyComponent.W);
            writer.WriteBool(keyComponent.A);
            writer.WriteBool(keyComponent.S);
            writer.WriteBool(keyComponent.D);
            ConverterHelper.WriteVector3(keyComponent.MousePosition, writer);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader) 
            => new KeyComponent
            {
                W = reader.ReadBool(),
                A = reader.ReadBool(),
                S = reader.ReadBool(),
                D = reader.ReadBool(),
                MousePosition = ConverterHelper.ReadVector3(reader)
            };
    }
}
