using System.Collections.Generic;
using System.Linq;
using ECS.Core;

namespace Assets.Scripts.Util
{
    public static class EcsHelper
    {
        public static IEnumerable<Entity> Filter<T>(this IEnumerable<Entity> entities)
            where T : ComponentBase 
            => entities.Where(e => e.Contain<T>());
    }
}
