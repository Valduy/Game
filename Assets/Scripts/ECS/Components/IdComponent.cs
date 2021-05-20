using ECS.Core;
using ECS.Serialization.Attributes;
using ECS.Serialization.Converters;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;

namespace Assets.Scripts.ECS.Components
{
    [ComponentConverter(typeof(IdComponentConverter))]
    public class IdComponent : ComponentBase
    {
        public uint Id;
    }

    public class IdComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var idComponent = (IdComponent) component;
            writer.WriteUInt32(idComponent.Id);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader) 
            => new IdComponent {Id = reader.ReadUInt32()};
    }
}
