using ECS.Core;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, определяющий здоровье сущности. 
    /// </summary>
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
}
