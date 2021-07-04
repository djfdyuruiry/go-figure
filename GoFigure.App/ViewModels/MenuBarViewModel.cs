using Caliburn.Micro;
using GoFigure.App.Model.Messages;
using System.Collections.Generic;
using System.Windows;

namespace GoFigure.App.ViewModels
{
    class MenuBarViewModel : BaseViewModel
    {
        public MenuBarViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        public async void StartNewGame() => 
            await PublishMessage(
                new NewGameStartedMessage
                {
                    Target = 122,
                    AvailableNumbers = new List<int> { 24, 5, 92, 1 }
                }
            );

        public void CloseApp() =>
            Application.Current.Shutdown();
    }
}
