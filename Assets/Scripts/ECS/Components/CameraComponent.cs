using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// Компонент, содержащий главную камеру.
    /// </summary>
    public class CameraComponent : ComponentBase
    {
        /// <summary>
        /// Главная камера.
        /// </summary>
        public Camera Camera;
    }
}
