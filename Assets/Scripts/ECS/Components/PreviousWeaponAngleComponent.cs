using ECS.Core;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, содержащий угол поворота оружия до перемещения.
    /// </summary>
    public class PreviousWeaponAngleComponent : ComponentBase
    {
        /// <summary>
        /// Угол поворота оружия до перемещения.
        /// </summary>
        public float PreviousAngle;
    }
}
