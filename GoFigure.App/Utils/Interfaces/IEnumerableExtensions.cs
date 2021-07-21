using System;
using System.Collections.Generic;
using System.Linq;

namespace GoFigure.App.Utils.Interfaces
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable) =>
            enumerable.OrderBy(_ => Guid.NewGuid());
    }
}
