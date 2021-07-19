using System.Collections.Generic;

namespace GoFigure.App.Utils
{
    public static class IDictionaryExtensions
    {
        public static U GetOrSet<T, U>(this IDictionary<T, U> dictionary, T key, U defaultValue) =>
            dictionary.ContainsKey(key)
                ? dictionary[key]
                : (dictionary[key] = defaultValue);
    }
}
