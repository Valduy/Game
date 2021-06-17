using ECS.Core;

    /// <summary>
    /// Компонент, содержащий угол поворота оружия до перемещения.
    /// </summary>
    public class WeaponPreviousAngleComponent : ComponentBase
    {
        /// <summary>
        /// Угол поворота оружия до перемещения.
        /// </summary>
        public float PreviousAngle;
    }
