using FakeItEasy;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;
using GoFigure.App.ViewModels.Menu;
using Xunit;

namespace GoFigure.Tests.ViewModels
{
  public class SkillMenuViewModelTests
  {
    private IEventAggregatorWrapper _eventAggregator;
    private IMessageBoxManager _messageBoxManager;
    private ISolutionGenerator _solutionGenerator;
    private GameSettings _gameSettings;

    private SkillMenuViewModel _viewModel;

    public SkillMenuViewModelTests()
    {
      _eventAggregator = A.Fake<IEventAggregatorWrapper>();
      _messageBoxManager = A.Fake<IMessageBoxManager>();
      _solutionGenerator = A.Fake<ISolutionGenerator>();
      _gameSettings = new GameSettings();

      _viewModel = new SkillMenuViewModel(
        _eventAggregator,
        _messageBoxManager,
        _solutionGenerator,
        _gameSettings
      );
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Event_Is_Published()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() => _eventAggregator.PublishOnCurrentThreadAsync(A<NewGameStartedMessage>._))
        .MustHaveHappened();
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Solution_Generator_Is_Called()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() => _solutionGenerator.Generate(A<int>._, A<int>._))
        .MustHaveHappened();
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Level_Passed_To_Solution_Generator_Is_Correct()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() =>
        _solutionGenerator.Generate(
          A<int>.That.IsEqualTo(1),
          A<int>._
        )
      ).MustHaveHappened();
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_NumberToIgnore_Passed_To_Solution_Generator_Is_Correct()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() =>
        _solutionGenerator.Generate(
          A<int>._,
          A<int>.That.IsEqualTo(-1)
        )
      ).MustHaveHappened();
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Published_Event_Has_Correct_Level()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() =>
        _eventAggregator.PublishOnCurrentThreadAsync(
          A<NewGameStartedMessage>.That.Matches(m => m.Level == 1)
        )
      ).MustHaveHappened();
    }


    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Published_Event_Has_Generated_Solution()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() =>
        _eventAggregator.PublishOnCurrentThreadAsync(
          A<NewGameStartedMessage>.That.Matches(m => m.Solution != null)
        )
      ).MustHaveHappened();
    }

    [Fact]
    public async void When_UseBeginnerSkill_Is_Called_And_Game_Is_Not_InProgess_Then_Settings_Event_Is_Published()
    {
      await _viewModel.UseBeginnerSkill(null);

      A.CallTo(() =>
        _eventAggregator.PublishOnCurrentThreadAsync(
          A<ZeroDataMessage>.That.Matches(m => m == ZeroDataMessage.GameSettingsChanged)
        )
      ).MustHaveHappened();
    }
  }
}
