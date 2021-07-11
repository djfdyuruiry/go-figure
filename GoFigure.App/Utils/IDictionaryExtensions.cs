using System.Collections.Generic;

namespace GoFigure.App.Utils
{
    public static class IDictionaryExtensions
    {
        public static U GetOrSet<T, U>(this IDictionary<T, U> dictionary, T key, U defaultValue)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = defaultValue;
            }

            return dictionary[key];
        }   
    }
}
