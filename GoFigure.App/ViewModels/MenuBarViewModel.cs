using Caliburn.Micro;
using System.Windows;

namespace GoFigure.App.ViewModels
{
    class MenuBarViewModel : BaseViewModel
    {
        public MenuBarViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public void StartNewGame()
        {
        }

        public void CloseApp() =>
            Application.Current.Shutdown();
    }
}
