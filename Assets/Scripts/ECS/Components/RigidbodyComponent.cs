using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, содержащий <see cref="Rigidbody2D"/> игрового объекта, описываемого сущностью.
    /// </summary>
    public class RigidbodyComponent : ComponentBase
    {
        /// <summary>
        /// <see cref="Rigidbody2D"/> игрового объекта.
        /// </summary>
        public Rigidbody2D Rigidbody;
    }
}
