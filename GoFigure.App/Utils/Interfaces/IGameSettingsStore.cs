using System.Threading.Tasks;

using GoFigure.App.Model.Settings;

namespace GoFigure.App.Utils.Interfaces
{
    public interface IGameSettingsStore
    {
        Task<GameSettings> Read();

        Task Write(GameSettings currentSettings);
    }
}
