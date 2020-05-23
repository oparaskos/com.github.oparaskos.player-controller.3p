using System.Collections.Generic;
using UnityEngine;

namespace Zeno.PlayerController
{
    public static class Extension
    {
        public static HashSet<T> ToHashSet<T>(
            this IEnumerable<T> source,
            IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }
        public static Vector2 XZ(
            this Vector3 source)
        {
            return new Vector2(source.x, source.z);
        }
    }
}