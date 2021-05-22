using ECS.Core;
using UnityEngine;

namespace Assets.Scripts.ECS.Components
{
    /// <summary>
    /// <see cref="UnityEngine.Transform"/> владельца сущности.
    /// </summary>
    /// <remarks>
    /// Это не означает, что данный <see cref="Transform"/> принадлежит родительскому <see cref="GameObject"/>.
    /// Владение здесь подразумевется "логическое", а не "физическое". Например, для камеры владельцем может быть игрок.
    /// При этом камера не обязательно должна быть дочерним объектом по отношению к игроку.
    /// </remarks>
    public class OwnerTransformComponent : ComponentBase
    {
        public Transform Transform;
    }
}
