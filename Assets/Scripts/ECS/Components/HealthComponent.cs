using ECS.Core;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, определяющий способность сущности получать урон. 
    /// </summary>
    public class HealthComponent : ComponentBase
    {
        /// <summary>
        /// Величина урона, которую сущность способна принять.
        /// </summary>
        public int Health;
    }
}
