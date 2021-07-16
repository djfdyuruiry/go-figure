using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;
using GoFigure.App.Model;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;

using static GoFigure.App.Constants;

namespace GoFigure.App.ViewModels.Menu
{
    public class OptionsMenuViewModel : SkillMenuViewModel,
                                 IHandle<NewGameStartedMessage>,
                                 IHandle<ZeroDataMessage>
    {
        private readonly ISoundEffectPlayer _soundEffectPlayer;
     
        private bool _hintEnabled;
        private bool _gameInProgess;

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
            MessageBoxManager messageBoxManager,
            ISoundEffectPlayer soundEffectPlayer,
            GameSettings gameSettings
        ) : base(eventAggregator, messageBoxManager, gameSettings) => 
            _soundEffectPlayer = soundEffectPlayer;

        public async void ToggleSound()
        {
            _gameSettings.SoundEnabled = !_gameSettings.SoundEnabled;
            _soundEffectPlayer.Enabled = _gameSettings.SoundEnabled;

            await PublishMessage(ZeroDataMessage.GameSettingsChanged);
        }

        public async void ShowSolutionHint() =>
            await PublishMessage(ZeroDataMessage.ShowSolutionHint);

        public async void ClearSolution() =>
            await PublishMessage(ZeroDataMessage.ClearSolution);

        public async void UseOperatorPrecedence() =>
            await SetOperatorPrecendence(true);

        public async void UseLeftToRightPrecedence() =>
            await SetOperatorPrecendence(false);

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            await base.HandleAsync(message, _);

            _gameInProgess = true;
            HintEnabled = true;
        }

        public async Task HandleAsync(ZeroDataMessage message, CancellationToken _)
        {
            await base.HandleAsync(message, _);

            if (!message.IsOneOf(
                ZeroDataMessage.NoHintsLeft,
                ZeroDataMessage.GameSettingsChanged
            ))
            {
                return;
            }

            if (message is ZeroDataMessage.GameSettingsChanged)
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
            if (_gameInProgess
                && _messageBoxManager.ShowOkCancel(OperatorPrecedenceChangeMessage) != MessageBoxResult.OK)
            {
                NotifyOfPropertyChange(() => OperatorPrecedence);
                NotifyOfPropertyChange(() => LeftToRightPrecedence);

                return;
            }

            _gameSettings.UseOperatorPrecedence = onFlag;

            await PublishMessage(ZeroDataMessage.GameSettingsChanged);
        }
    }
}
