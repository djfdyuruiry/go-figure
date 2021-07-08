using System.Threading;
using System.Threading.Tasks;

using Caliburn.Micro;

using GoFigure.App.Model;
using GoFigure.App.Model.Messages;

namespace GoFigure.App.ViewModels.Menu
{
    class OptionsMenuViewModel : SkillMenuViewModel,
                                 IHandle<NewGameStartedMessage>,
                                 IHandle<ZeroDataMessages>
    {
        private readonly GameSettings _gameSettings;

        private bool _hintEnabled;

        public bool SoundEnabled => _gameSettings.SoundEnabled;

        public bool HintEnabled
        {
            get => _hintEnabled;
            set
            {
                _hintEnabled = value;

                NotifyOfPropertyChange(() => HintEnabled);
            }
        }

        public bool OperatorPrecedence => _gameSettings.UseOperatorPrecedence;

        public bool LeftToRightPrecedence => !_gameSettings.UseOperatorPrecedence;

        public OptionsMenuViewModel(
            IEventAggregator eventAggregator,
            GameSettings gameSettings
        ) : base(eventAggregator, gameSettings) =>
            _gameSettings = gameSettings;

        public async void ToggleSound()
        {
            _gameSettings.SoundEnabled = !_gameSettings.SoundEnabled;

            await PublishMessage(ZeroDataMessages.GameSettingsChanged);
        }

        public async void ShowSolutionHint() =>
            await PublishMessage(ZeroDataMessages.ShowSolutionHint);

        public async void ClearSolution() =>
            await PublishMessage(ZeroDataMessages.ClearSolution);

        public async void UseOperatorPrecedence() =>
            await SetOperatorPrecendence(true);

        public async void UseLeftToRightPrecedence() =>
            await SetOperatorPrecendence(false);

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _) =>
            HintEnabled = true;

        public async Task HandleAsync(ZeroDataMessages message, CancellationToken _)
        {
            await base.HandleAsync(message, _);

            if (message != ZeroDataMessages.NoHintsLeft
                && message != ZeroDataMessages.GameSettingsChanged)
            {
                return;
            }

            if (message == ZeroDataMessages.GameSettingsChanged)
            {
                NotifyOfPropertyChange(() => SoundEnabled);
                NotifyOfPropertyChange(() => OperatorPrecedence);
                NotifyOfPropertyChange(() => LeftToRightPrecedence);

                return;
            }

            HintEnabled = false;
        }

        private async Task SetOperatorPrecendence(bool onFlag)
        {
            _gameSettings.UseOperatorPrecedence = onFlag;

            await PublishMessage(ZeroDataMessages.GameSettingsChanged);
        }
    }
}
