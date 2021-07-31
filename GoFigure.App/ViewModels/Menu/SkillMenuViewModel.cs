using System;
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
  public class SkillMenuViewModel : HelpMenuViewModel,
                    ISkillMenuViewModel,
                    IHandle<NewGameStartedMessage>,
                    IHandle<ZeroDataMessage>
  {
    protected readonly IMessageBoxManager _messageBoxManager;
    protected readonly ISolutionGenerator _generator;
    protected readonly GameSettings _gameSettings;

    private bool _gameInProgess;

    public bool BeginnerSkill => _gameSettings.CurrentSkill == Skill.Beginner;

    public bool IntermediateSkill => _gameSettings.CurrentSkill == Skill.Intermediate;

    public bool ExpertSkill => _gameSettings.CurrentSkill == Skill.Expert;

    public SkillMenuViewModel(
      IEventAggregatorWrapper eventAggregator,
      IMessageBoxManager messageBoxManager,
      ISolutionGenerator generator,
      GameSettings gameSettings
    ) : base(eventAggregator)
    {
      _messageBoxManager = messageBoxManager;
      _generator = generator;
      _gameSettings = gameSettings;
    }

    public async Task UseBeginnerSkill(DependencyObject view) =>
      await SetSkill(view, Skill.Beginner);

    public async Task UseIntermediateSkill(DependencyObject view) =>
      await SetSkill(view, Skill.Intermediate);

    public async Task UseExpertSkill(DependencyObject view) =>
      await SetSkill(view, Skill.Expert);

    private async Task SetSkill(DependencyObject view, Skill skill)
    {
      Console.WriteLine("Hello");

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
        NotifyOfPropertyChange(() => BeginnerSkill);
        NotifyOfPropertyChange(() => IntermediateSkill);
        NotifyOfPropertyChange(() => ExpertSkill);

        return;
      }

      _gameSettings.CurrentSkill = skill;

      await PublishMessage(ZeroDataMessage.GameSettingsChanged);

      if (_gameInProgess)
      {
        await PublishNewGameMessage();
      }
    }

    public async Task PublishNewGameMessage() =>
      await PublishMessage(
        new NewGameStartedMessage
        {
          Level = 1,
          Solution = _generator.Generate(1, -1)
        }
      );

    public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _) =>
      _gameInProgess = true;

    public async Task HandleAsync(ZeroDataMessage message, CancellationToken _)
    {
      if (message != ZeroDataMessage.GameSettingsChanged)
      {
        return;
      }

      NotifyOfPropertyChange(() => BeginnerSkill);
      NotifyOfPropertyChange(() => IntermediateSkill);
      NotifyOfPropertyChange(() => ExpertSkill);
    }
  }
}
