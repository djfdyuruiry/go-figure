using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    class AppScreenViewModel : Screen
    {
        public MenuBarViewModel MenuBar { get; private set; }

        public GameViewModel Game { get; private set; }

        public AppScreenViewModel(MenuBarViewModel menuBar, GameViewModel game)
        {
            MenuBar = menuBar;
            Game = game;
        }
    }
}
