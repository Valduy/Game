using ECS.Core;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент определяет, на сколько градусов должно было повернуться оружие, чтобы нанести урон.
    /// </summary>
    public class WeaponEffectiveDeltaComponent : ComponentBase
    {
        /// <summary>
        /// Эффективная дельта в градусах.
        /// </summary>
        public float Delta;
    }
}
