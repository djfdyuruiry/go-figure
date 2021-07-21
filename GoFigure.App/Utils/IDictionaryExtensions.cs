using System;
using System.Collections.Generic;

namespace GoFigure.App.Utils
{
    public static class IDictionaryExtensions
    {
        public static U GetOrSet<T, U>(this IDictionary<T, U> dictionary, T key, U defaultValue) =>
            dictionary.ContainsKey(key)
                ? dictionary[key]
                : (dictionary[key] = defaultValue);

        public static void Foreach<T, U>(this IDictionary<T, U> dictionary, Action<T, U> action)
        {
            foreach (var kvp in dictionary)
            {
                action(kvp.Key, kvp.Value);
            }
        }
    }
}
