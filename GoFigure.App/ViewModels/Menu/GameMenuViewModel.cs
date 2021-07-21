using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;

using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;
using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Menu
{
    public class GameMenuViewModel : OptionsMenuViewModel,
                                     IGameMenuViewModel,
                                     IHandle<NewGameStartedMessage>,
                                     IHandle<ZeroDataMessage>
    {
        private readonly IWindowManager _windowManager;
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
            IEventAggregatorWrapper eventAggregator,
            IWindowManager windowManager,
            IMessageBoxManager messageBoxManager,
            ISolutionGenerator generator,
            ISoundEffectPlayer soundEffectPlayer,
            HighScoresScreenViewModel highScores,
            GameSettings gameSettings
        ) : base(eventAggregator, messageBoxManager, soundEffectPlayer, generator, gameSettings)
        {
            _windowManager = windowManager;
            _highScores = highScores;
        }

        public async void StartNewGame() =>
            await PublishNewGameMessage();

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

        public new async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            await base.HandleAsync(message, _);

            CanPause = true;
        }

        public new async Task HandleAsync(ZeroDataMessage message, CancellationToken _)
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
