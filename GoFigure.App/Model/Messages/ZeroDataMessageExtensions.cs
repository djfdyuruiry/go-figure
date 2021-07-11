using System.Linq;

namespace GoFigure.App.Model.Messages
{
    public static class ZeroDataMessageExtensions
    {
        public static bool IsOneOf(this ZeroDataMessage message, params ZeroDataMessage[] types) =>
            types.Any(t => message == t);
    }
}
