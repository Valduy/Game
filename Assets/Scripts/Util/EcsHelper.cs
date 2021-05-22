using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ECS.Components;
using ECS.Core;

namespace Assets.Scripts.Util
{
    public static class EcsHelper
    {
        public static IEnumerable<Entity> Filter<T>(this IEnumerable<Entity> entities)
            where T : ComponentBase 
            => entities.Where(e => e.Contain<T>());

        public static Dictionary<uint, Entity> GetIdentifiedEntities(this Engine engine)
            => engine.GetEntities()
                .Filter<IdComponent>()
                .ToDictionary(e => e.Get<IdComponent>().Id, e => e);

        public static uint Id(this Entity entity) 
            => entity.Get<IdComponent>().Id;
    }
}
