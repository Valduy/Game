using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class UnityHelper
    {
        public static T GetChildComponent<T>(GameObject go) where T : Component
        {
            if (go.TryGetComponent(typeof(T), out var component))
            {
                return (T)component;
            }

            foreach (Transform child in go.transform)
            {
                var componentT = GetChildComponent<T>(child.gameObject);

                if (componentT != null)
                {
                    return componentT;
                }
            }

            return default(T);
        }
    }
}
