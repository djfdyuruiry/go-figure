using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

using GoFigure.App.Model.Messages;
using GoFigure.App.ViewModels.Controls;

namespace GoFigure.Tests.ViewModels.Controls
{
  public class LevelMeterViewModelTests : ViewModelTestsBase<LevelMeterViewModel>
  {
    private List<Func<string>> _levelFillLookups;

    public LevelMeterViewModelTests() : base()
    {
      _viewModel = new LevelMeterViewModel(_eventAggregator);

      _levelFillLookups = new List<Func<string>>
      {
        () => _viewModel.Level2Fill,
        () => _viewModel.Level3Fill,
        () => _viewModel.Level4Fill,
        () => _viewModel.Level5Fill,
        () => _viewModel.Level6Fill,
        () => _viewModel.Level7Fill,
        () => _viewModel.Level8Fill,
        () => _viewModel.Level9Fill,
        () => _viewModel.Level10Fill
      };
    }

    [Fact]
    public void When_ViewModel_Created_Then_All_Levels_Are_Blank() => 
      Assert.True(
        _levelFillLookups.TrueForAll(l => "Transparent" == l())
      );

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public async Task When_NewGameStarted_Event_Recieved_Then_Levels_Above_Event_Level_Are_Blank(int level)
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage
      {
        Level = level
      }, new CancellationToken());

      Enumerable.Range(level + 1, 10 - level)
        .ToList()
        .ForEach(l =>
          Assert.True(
            "Transparent" == _levelFillLookups[l - 2]()
          )
        );
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public async Task When_NewGameStarted_Event_Recieved_Then_Levels_Up_To_Event_Level_Are_Filled_In(int level)
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage
      {
        Level = level
      }, new CancellationToken());

      Enumerable.Range(0, level - 2)
        .ToList()
        .ForEach(l =>
          Assert.True(
            "Red" == _levelFillLookups[l]()
          )
        );
    }
  }
}
