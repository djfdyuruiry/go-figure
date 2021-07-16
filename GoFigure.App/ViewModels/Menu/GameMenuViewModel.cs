using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels.Menu
{
    public class GameMenuViewModel : OptionsMenuViewModel,
                              IHandle<NewGameStartedMessage>,
                              IHandle<ZeroDataMessage>
    {
        private readonly IWindowManager _windowManager;
        private readonly SolutionGenerator _generator;
        private readonly HighScoresScreenViewModel _highScores;

        private bool _gamePaused;
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
            IWindowManager windowManager,
            MessageBoxManager messageBoxManager,
            SolutionGenerator generator,
            ISoundEffectPlayer soundEffectPlayer,
            HighScoresScreenViewModel highScores,
            GameSettings gameSettings
        ) : base(eventAggregator, messageBoxManager, soundEffectPlayer, gameSettings)
        {
            _windowManager = windowManager;
            _generator = generator;
            _highScores = highScores;
        }

        public async void StartNewGame() =>
            await PublishNewGameMessage();

        public async Task PublishNewGameMessage() =>
            await PublishMessage(
                new NewGameStartedMessage
                {
                    Level = 1,
                    Solution = _generator.Generate(1, -1)
                }
            );

        public async void PauseOrResumeGame() =>
            await PublishPauseOrResumeGameMessage();

        public async Task PublishPauseOrResumeGameMessage()
        {
            if (_gamePaused)
            {
                await PublishMessage(ZeroDataMessage.ResumeGame);
                return;
            }

            await PublishMessage(ZeroDataMessage.PauseGame);
        }

        public async void ShowHighScores() =>
            await _windowManager.ShowWindowAsync(_highScores);

        public void CloseApp() =>
            Application.Current.Shutdown();

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            await base.HandleAsync(message, _);

            CanPause = true;
        }

        public async Task HandleAsync(ZeroDataMessage message, CancellationToken _)
        {
            await base.HandleAsync(message, _);

            if (!message.IsOneOf(
                ZeroDataMessage.PauseGame,
                ZeroDataMessage.ResumeGame
            ))
            {
                return;
            }

            _gamePaused = message is ZeroDataMessage.PauseGame;
        }
    }
}
