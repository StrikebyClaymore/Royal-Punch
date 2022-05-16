using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtension
    {
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, bool includeInactive, out T component)
        {
            component = gameObject.GetComponentInChildren<T>(includeInactive);
            return component == null;
        }
    }
}