using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

using GoFigure.App.Model;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Solution;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels.Controls
{
  public class ControlsViewModel : BaseControlViewModel,
                   IControlsViewModel,
                   IHandle<NewGameStartedMessage>,
                   IHandle<ZeroDataMessage>
  {
    private const string NumberPlaceholder = "?";

    private IList<int> _numbers;
    private bool _controlsEnabled;
    private bool _hintEnabled;

    public string Number1 => NumberOrPlaceholder(0);

    public string Number2 => NumberOrPlaceholder(1);

    public string Number3 => NumberOrPlaceholder(2);

    public string Number4 => NumberOrPlaceholder(3);

    public bool ControlsEnabled
    {
      get => _controlsEnabled;
      set
      {
        _controlsEnabled = value;

        NotifyOfPropertyChange(() => ControlsEnabled);

        NotifyOfPropertyChange(() => HintEnabled);

        NotifyOfPropertyChange(() => Number1);
        NotifyOfPropertyChange(() => Number2);
        NotifyOfPropertyChange(() => Number3);
        NotifyOfPropertyChange(() => Number4);
      }
    }

    public bool HintEnabled
    {
      get => _hintEnabled && _controlsEnabled;
      set
      {
        _hintEnabled = value;

        NotifyOfPropertyChange(() => HintEnabled);
      }
    }

    public ControlsViewModel(IEventAggregatorWrapper eventAggregator) : base(eventAggregator)
    {
      _numbers = new List<int>();

      ControlsEnabled = false;
      HintEnabled = false;
    }

    public async Task EnterNumberIntoSolution(int numberIndex) =>
      await PublishMessage(
        new SetSolutionSlotMessage
        {
          Value = new NumberSlotValue
          {
            Value = _numbers[numberIndex]
          }
        }
      );

    public async Task EnterOperatorIntoSolution(char operatorSymbol) =>
      await PublishMessage(
        new SetSolutionSlotMessage
        {
          Value = new OperatorSlotValue
          {
            Value = operatorSymbol.ToOperator()
          }
        }
      );

    public async Task SubmitSolution(DependencyObject view) =>
      await PublishMessage(
        new SubmitSolutionMessage
        {
          Source = view
        }
      );

    public async Task ShowSolutionHint() =>
      await PublishMessage(ZeroDataMessage.ShowSolutionHint);

    public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
    {
      _numbers = message.Solution
        .AvailableNumbers
        .Shuffle()
        .ToList();

      ControlsEnabled = true;
      HintEnabled = true;
    }

    public async Task HandleAsync(ZeroDataMessage message, CancellationToken _)
    {
      if (!message.IsOneOf(
        ZeroDataMessage.NoHintsLeft,
        ZeroDataMessage.PauseGame,
        ZeroDataMessage.ResumeGame
      ))
      {
        return;
      }

      if (message is ZeroDataMessage.NoHintsLeft)
      {
        HintEnabled = false;
      }
      else
      {
        ControlsEnabled = message != ZeroDataMessage.PauseGame;
      }
    }

    private string NumberOrPlaceholder(int index) =>
      !ControlsEnabled
      || _numbers.Count == 0
      || _numbers.Count - 1 < index
        ? NumberPlaceholder
        : $"{_numbers[index]}";
  }
}
