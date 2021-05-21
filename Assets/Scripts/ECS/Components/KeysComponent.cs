using ECS.Core;
using ECS.Serialization.Attributes;
using ECS.Serialization.Converters;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;

namespace Assets.Scripts.ECS.Components
{
    [ComponentConverter(typeof(KeysComponentConverter))]
    public class KeysComponent : ComponentBase
    {
        public bool W;
        public bool A;
        public bool S;
        public bool D;
    }

    public class KeysComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var keyComponent = (KeysComponent) component;
            writer.WriteBool(keyComponent.W);
            writer.WriteBool(keyComponent.A);
            writer.WriteBool(keyComponent.S);
            writer.WriteBool(keyComponent.D);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader) 
            => new KeysComponent
            {
                W = reader.ReadBool(),
                A = reader.ReadBool(),
                S = reader.ReadBool(),
                D = reader.ReadBool(),
            };
    }
}
