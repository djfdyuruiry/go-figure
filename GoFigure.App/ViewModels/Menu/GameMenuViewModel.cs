using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;

using GoFigure.App.Model;
using GoFigure.App.Model.Messages;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels.Menu
{
    class GameMenuViewModel : OptionsMenuViewModel, IHandle<NewGameStartedMessage>
    {
        private readonly SolutionGenerator _generator;

        private bool _canPause;

        public bool CanPause
        {
            get => _canPause;
            set
            {
                _canPause = value;

                NotifyOfPropertyChange(() => CanPause);
            }
        }

        public GameMenuViewModel(
            IEventAggregator eventAggregator,
            SolutionGenerator generator,
            GameSettings gameSettings
        ) : base(eventAggregator, gameSettings)
        {
            _generator = generator;
        }

        public async void StartNewGame() =>
            await PublishNewGameMessage();

        public async Task PublishNewGameMessage() =>
            await PublishMessage(
                new NewGameStartedMessage
                {
                    Level = 1,
                    Solution = _generator.Generate(1)
                }
            );

        public async void PauseGame() =>
            await PublishPauseGameMessage();

        public async Task PublishPauseGameMessage() =>
            await PublishMessage(ZeroDataMessages.PauseGame);

        public void CloseApp() =>
            Application.Current.Shutdown();

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            await base.HandleAsync(message, _);

            CanPause = true;
        }
    }
}
