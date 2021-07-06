using System.Windows;

using GoFigure.App.Model.Messages;
using GoFigure.App.Utils;

using Caliburn.Micro;

namespace GoFigure.App.ViewModels
{
    class MenuBarViewModel : BaseViewModel
    {
        private readonly SolutionGenerator _generator;

        public MenuBarViewModel(IEventAggregator eventAggregator, SolutionGenerator generator) : base(eventAggregator)
        {
            _generator = generator;
        }

        public async void StartNewGame() => 
            await PublishMessage(
                new NewGameStartedMessage
                {
                    Solution = _generator.Generate()
                }
            );

        public void CloseApp() =>
            Application.Current.Shutdown();
    }
}
