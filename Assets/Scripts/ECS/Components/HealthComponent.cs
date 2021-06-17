using ECS.Core;
using ECS.Serialization.Attributes;
using ECS.Serialization.Converters;
using ECS.Serialization.Readers;
using ECS.Serialization.Writers;

    /// <summary>
    /// Компонент, определяющий здоровье сущности. 
    /// </summary>
    [ComponentConverter(typeof(HealthComponentConverter))]
    public class HealthComponent : ComponentBase
    {
        /// <summary>
        /// Максимальное здоровье.
        /// </summary>
        public int MaxHealth;
        /// <summary>
        /// Текущее здоровье.
        /// </summary>
        public int CurrentHealth;
    }

    public class HealthComponentConverter : IComponentConverter
    {
        public void ToTokensSequence(object component, ISequentialWriter writer)
        {
            var healthComponent = (HealthComponent) component;
            writer.WriteInt32(healthComponent.MaxHealth);
            writer.WriteInt32(healthComponent.CurrentHealth);
        }

        public ComponentBase FromTokenSequence(ISequentialReader reader) 
            => new HealthComponent
            {
                MaxHealth = reader.ReadInt32(),
                CurrentHealth = reader.ReadInt32()
            };
    }
