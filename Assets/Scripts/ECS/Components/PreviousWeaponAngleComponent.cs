using ECS.Core;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Угол поворота оружия до перемещения.
    /// </summary>
    public class PreviousWeaponAngleComponent : ComponentBase
    {
        public float PreviousAngle;
    }
}
