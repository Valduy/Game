using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, определяющий направление движения.
    /// </summary>
    public class DirectionComponent : ComponentBase
    {
        /// <summary>
        /// Направление движения.
        /// </summary>
        public Vector2 Direction;
    }
}
