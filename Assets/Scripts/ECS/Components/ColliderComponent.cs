using ECS.Core;
using UnityEngine;


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

