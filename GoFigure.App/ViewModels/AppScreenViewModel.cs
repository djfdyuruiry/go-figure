using System.Windows.Input;

using Caliburn.Micro;

using GoFigure.App.ViewModels.Menu;

namespace GoFigure.App.ViewModels
{
    public class AppScreenViewModel : BaseScreenViewModel
    {
        public MenuBarViewModel MenuBar { get; private set; }

        public GameViewModel Game { get; private set; }

        public AppScreenViewModel(
            IEventAggregator eventAggregator,
            MenuBarViewModel menuBar,
            GameViewModel game
        ) : base(eventAggregator)
        {
            MenuBar = menuBar;
            Game = game;
        }

        public async void KeyPressed(KeyEventArgs e)
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
    }
}
