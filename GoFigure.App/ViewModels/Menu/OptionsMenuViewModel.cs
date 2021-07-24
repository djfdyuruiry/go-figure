using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;

using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;
using static GoFigure.App.Constants;

namespace GoFigure.App.ViewModels.Menu
{
  public class OptionsMenuViewModel : SkillMenuViewModel,
                    IOptionsMenuViewModel,
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
      IEventAggregatorWrapper eventAggregator,
      IMessageBoxManager messageBoxManager,
      ISoundEffectPlayer soundEffectPlayer,
      ISolutionGenerator solutionGenerator,
      GameSettings gameSettings
    ) : base(eventAggregator, messageBoxManager, solutionGenerator, gameSettings) =>
      _soundEffectPlayer = soundEffectPlayer;

    public async Task ToggleSound()
    {
      _gameSettings.SoundEnabled = !_gameSettings.SoundEnabled;
      _soundEffectPlayer.Enabled = _gameSettings.SoundEnabled;

      await PublishMessage(ZeroDataMessage.GameSettingsChanged);
    }

    public async Task ShowSolutionHint() =>
      await PublishMessage(ZeroDataMessage.ShowSolutionHint);

    public async Task ClearSolution() =>
      await PublishMessage(ZeroDataMessage.ClearSolution);

    public async Task UseOperatorPrecedence(DependencyObject view) =>
      await SetOperatorPrecendence(view, true);

    public async Task UseLeftToRightPrecedence(DependencyObject view) =>
      await SetOperatorPrecendence(view, false);

    public new async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
    {
      await base.HandleAsync(message, _);

      _gameInProgess = true;
      HintEnabled = true;
    }

    public new async Task HandleAsync(ZeroDataMessage message, CancellationToken _)
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

    private async Task SetOperatorPrecendence(DependencyObject view, bool onFlag)
    {
      var okToProceed = !_gameInProgess;

      if (!okToProceed)
      {
        okToProceed = _messageBoxManager.ShowOkCancel(
          view,
          OperatorPrecedenceChangeMessage
        ) == MessageBoxResult.OK;
      }

      if (!okToProceed)
      {
        NotifyOfPropertyChange(() => OperatorPrecedence);
        NotifyOfPropertyChange(() => LeftToRightPrecedence);

        return;
      }

      _gameSettings.UseOperatorPrecedence = onFlag;

      await PublishMessage(ZeroDataMessage.GameSettingsChanged);
      await PublishNewGameMessage();
    }
  }
}
