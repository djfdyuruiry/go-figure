using System.Threading.Tasks;

using GoFigure.App.Model.Settings;

namespace GoFigure.App.Utils
{
    public interface IGameSettingsStore
    {
        Task<GameSettings> Read();

        Task Write(GameSettings currentSettings);
    }
}
