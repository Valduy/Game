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
    /// Ввод мыши.
    /// </summary>
    [ComponentConverter(typeof(MouseComponentConverter))]
    public class MouseComponent : ComponentBase
    {
        /// <summary>
        /// Позиция курсора в мировых координатах.
        /// </summary>
        public Vector3 MousePosition;
    }

    public class MouseComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var mouseComponent = (MouseComponent) component;
            ConverterHelper.WriteVector3(mouseComponent.MousePosition, writer);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader) 
            => new MouseComponent {MousePosition = ConverterHelper.ReadVector3(reader)};
    }
}
