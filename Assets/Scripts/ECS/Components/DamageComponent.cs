using ECS.Core;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, определяющий урон сущности, способной наносить урон.
    /// </summary>
    public class DamageComponent : ComponentBase
    {
        /// <summary>
        /// Величина урона.
        /// </summary>
        public int Damage;
    }
}
