using System.Windows.Input;

using Caliburn.Micro;

using GoFigure.App.Utils;
using GoFigure.App.ViewModels.Menu;

namespace GoFigure.App.ViewModels
{
    class AppScreenViewModel : Screen
    {
        private SolutionGenerator _generator;

        public MenuBarViewModel MenuBar { get; private set; }

        public GameViewModel Game { get; private set; }

        public AppScreenViewModel(
            SolutionGenerator generator,
            MenuBarViewModel menuBar,
            GameViewModel game
        )
        {
            _generator = generator;

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
                await MenuBar.StartNewGame();
            }
            else if (e.Key == Key.F3)
            {
                await MenuBar.PauseGame();
            }
        }
    }
}
