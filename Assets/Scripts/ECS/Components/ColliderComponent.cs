using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, содержаций коллайдер <see cref="GameObject"/>.
    /// </summary>
    public class ColliderComponent : ComponentBase
    {
        /// <summary>
        /// Коллайдер <see cref="GameObject"/>.
        /// </summary>
        public Collider2D Collider;
    }
}
