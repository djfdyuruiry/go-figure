using System.Threading;
using System.Threading.Tasks;

using Xunit;

using GoFigure.App.Model.Messages;
using GoFigure.App.ViewModels.Controls;
using GoFigure.App.Model.Solution;
using System.Collections.Generic;
using GoFigure.App.Model;

namespace GoFigure.Tests.ViewModels.Menu
{
  public class ControlsViewModelTests : ViewModelTestsBase<ControlsViewModel>
  {
    public ControlsViewModelTests() => _viewModel =
      new ControlsViewModel(_eventAggregator);

    [Fact]
    public void When_ViewModel_Is_Constructed_Then_It_Subscribes_To_Events() =>
      RunEventSubscribeTest();

    [Fact]
    public void When_ViewModel_Is_Constructed_Then_Controls_Are_Disabled() =>
      Assert.False(_viewModel.ControlsEnabled);

    [Fact]
    public void When_ViewModel_Is_Constructed_Then_Hints_Are_Disabled() =>
      Assert.False(_viewModel.HintEnabled);

    [Fact]
    public void When_ViewModel_Is_Constructed_Then_Numbers_Use_Placeholders()
    {
      Assert.Equal("?", _viewModel.Number1);
      Assert.Equal("?", _viewModel.Number2);
      Assert.Equal("?", _viewModel.Number3);
      Assert.Equal("?", _viewModel.Number4);
    }

    [Fact]
    public async Task When_NewGameStarted_Event_Is_Recieved_Then_Controls_Are_Enabled()
    {
      await PublishNewGameMessage();

      Assert.True(_viewModel.ControlsEnabled);
    }

    [Fact]
    public async Task When_NewGameStarted_Event_Is_Recieved_Then_Hints_Are_Enabled()
    {
      await PublishNewGameMessage();

      Assert.True(_viewModel.HintEnabled);
    }

    [Fact]
    public async Task When_NewGameStarted_Event_Is_Recieved_Then_Numbers_Match_Solution_Plan()
    {
      await PublishNewGameMessage();

      var expectedNumbers = new List<string> { "25", "4", "4", "7" };
      var numbers = new List<string> { _viewModel.Number1, _viewModel.Number2, _viewModel.Number3, _viewModel.Number4 };

      numbers.Sort();

      Assert.Equal(expectedNumbers, numbers);
    }

    private async Task PublishNewGameMessage() =>
      await _viewModel.HandleAsync
      (
        new NewGameStartedMessage
        {
          Level = 2,
          Solution = new SolutionPlan
          {
            Slots = new List<ISolutionSlotValue>
            {
              new NumberSlotValue { Value = 4 },
              new OperatorSlotValue { Value = Operator.Multiply },
              new NumberSlotValue { Value = 7 },
              new OperatorSlotValue { Value = Operator.Add },
              new NumberSlotValue { Value = 25 },
              new OperatorSlotValue { Value = Operator.Divide },
              new NumberSlotValue { Value = 4 },
            }
          }
        },
        new CancellationToken()
      );
  }
}
