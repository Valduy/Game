using ECS.Core;
using UnityEngine;

    /// <summary>
    /// Компонент, содержащий <see cref="UnityEngine.Transform"/> владельца сущности.
    /// </summary>
    /// <remarks>
    /// Это не означает, что данный <see cref="Transform"/> принадлежит родительскому <see cref="GameObject"/>.
    /// Владение здесь подразумевется "логическое", а не "физическое". Например, для камеры владельцем может быть игрок.
    /// При этом камера не обязательно должна быть дочерним объектом по отношению к игроку.
    /// </remarks>
    public class OwnerTransformComponent : ComponentBase
    {
        /// <summary>
        /// <see cref="UnityEngine.Transform"/> владельца.
        /// </summary>
        public Transform Transform;
    }
