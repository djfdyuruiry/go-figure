using Caliburn.Micro;
using System.Windows;

namespace GoFigure.App.ViewModels
{
    class MenuBarViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _eventAggregator;

        public MenuBarViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        public void StartNewGame()
        {
        }

        public void CloseApp() =>
            Application.Current.Shutdown();
    }
}
