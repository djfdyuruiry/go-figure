using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;

using GoFigure.App.Model.Messages;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels.Menu
{
    class GameMenuViewModel : OptionsMenuViewModel
    {
        private readonly SolutionGenerator _generator;

        public GameMenuViewModel(
            IEventAggregator eventAggregator,
            SolutionGenerator generator
        ) : base(eventAggregator)
        {
            _generator = generator;
        }

        public async Task StartNewGame() =>
            await PublishMessage(
                new NewGameStartedMessage
                {
                    Level = 1,
                    Solution = _generator.Generate(1)
                }
            );

        public async Task PauseGame() =>
            await PublishMessage(ZeroDataMessages.PauseGame);

        public void CloseApp() =>
            Application.Current.Shutdown();
    }
}
