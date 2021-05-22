using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, содержащий <see cref="UnityEngine.Transform"/> игрового объекта, описываемого сущностью.
    /// </summary>
    public class TransformComponent : ComponentBase
    {
        /// <summary>
        /// <see cref="UnityEngine.Transform"/> игрового объекта.
        /// </summary>
        public Transform Transform;
    }
}
