using System.Threading.Tasks;
using System.Windows.Input;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels
{
  public class AppScreenViewModel : BaseScreenViewModel, IAppScreenViewModel
  {
    private readonly IGameSettingsStore _gameSettingsStore;
    private readonly GameSettings _gameSettings;

    public IMenuBarViewModel MenuBar { get; private set; }

    public IGameViewModel Game { get; private set; }

    public AppScreenViewModel(
      IEventAggregatorWrapper eventAggregator,
      IGameSettingsStore gameSettingsStore,
      GameSettings gameSettings,
      IMenuBarViewModel menuBar,
      IGameViewModel game
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
