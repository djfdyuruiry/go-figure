using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    class AppScreenViewModel : Screen
    {
        private int _score;
        private string _time;
        private int _target;

        public MenuBarViewModel MenuBar { get; private set; }

        public int Score 
        {
            get => _score;
            set
            {
                _score = value;

                NotifyOfPropertyChange(() => Score);
            }
        }

        public string Time
        {
            get => _time;
            set
            {
                _time = value;

                NotifyOfPropertyChange(() => Time);
            }
        }

        public int Target
        {
            get => _target;
            set
            {
                _target = value;

                NotifyOfPropertyChange(() => Target);
            }
        }

        public AppScreenViewModel(MenuBarViewModel menuBarViewModel)
        {
            MenuBar = menuBarViewModel;

            Time = "00:00";
        }
    }
}
