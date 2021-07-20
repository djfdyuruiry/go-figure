using System.Threading.Tasks;
using System.Windows.Input;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;
using GoFigure.App.ViewModels.Menu;

namespace GoFigure.App.ViewModels
{
    public class AppScreenViewModel : BaseScreenViewModel
    {
        private readonly IGameSettingsStore _gameSettingsStore;
        private readonly GameSettings _gameSettings;

        public MenuBarViewModel MenuBar { get; private set; }

        public GameViewModel Game { get; private set; }

        public AppScreenViewModel(
            IEventAggregatorWrapper eventAggregator,
            IGameSettingsStore gameSettingsStore,
            GameSettings gameSettings,
            MenuBarViewModel menuBar,
            GameViewModel game
        ) : base(eventAggregator)
        {
            _gameSettingsStore = gameSettingsStore;
            _gameSettings = gameSettings;

            MenuBar = menuBar;
            Game = game;
        }

        public async Task KeyPressed(KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                MenuBar.ShowHelp();
            }
            else if (e.Key == Key.F2)
            {
                await MenuBar.PublishNewGameMessage();
            }
            else if (e.Key == Key.F3)
            {
                await MenuBar.PublishPauseOrResumeGameMessage();
            }
        }

        public async void SaveCurrentSettings() =>
            await _gameSettingsStore.Write(_gameSettings);
    }
}
