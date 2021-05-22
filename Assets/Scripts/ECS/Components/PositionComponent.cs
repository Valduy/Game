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
    /// Компонент, содержащий позицию <see cref="GameObject"/>, описываемого сущностью.
    /// </summary>
    [ComponentConverter(typeof(PositionComponentConverter))]
    public class PositionComponent : ComponentBase
    {
        /// <summary>
        /// Позиция <see cref="GameObject"/>.
        /// </summary>
        public Vector3 Position;
    }

    public class PositionComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var positionComponent = (PositionComponent)component;
            ConverterHelper.WriteVector3(positionComponent.Position, writer);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader)
            => new PositionComponent { Position = ConverterHelper.ReadVector3(reader) };
    }
}
