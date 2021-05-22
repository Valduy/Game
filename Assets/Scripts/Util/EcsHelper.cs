using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.Util
{
    public static class EcsHelper
    {
        /// <summary>
        /// Метод позволяет отфильтровать сущности по наличию компонента <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип компонента.</typeparam>
        /// <param name="entities">Фильтруемые сущности.</param>
        /// <returns>Сущности с компонентом <see cref="T"/>.</returns>
        public static IEnumerable<Entity> Filter<T>(this IEnumerable<Entity> entities)
            where T : ComponentBase 
            => entities.Where(e => e.Contain<T>());

        /// <summary>
        /// Метод отображает сущности <see cref="Engine"/> с компонентом <see cref="IdComponent"/> в словарь.  
        /// </summary>
        /// <param name="engine"><see cref="Engine"/>.</param>
        /// <returns>Словарь идентифицируемых сущностей, где ключом является id, значением - сущность.</returns>
        public static Dictionary<uint, Entity> GetIdentifiedEntities(this Engine engine)
            => engine.GetEntities()
                .Filter<IdComponent>()
                .ToDictionary(e => e.Get<IdComponent>().Id, e => e);

        /// <summary>
        /// Метод получает id сущности (используется компонент <see cref="IdComponent"/>).
        /// </summary>
        /// <param name="entity">Сущность, id которой необходимо определить.</param>
        /// <returns>id компонента.</returns>
        /// <exception cref="System.NullReferenceException">Если <see cref="IdComponent"/> отсутствует.</exception>
        public static uint Id(this Entity entity) 
            => entity.Get<IdComponent>().Id;
    }
}
