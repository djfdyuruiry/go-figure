using System.Windows;

using FakeItEasy;
using Xunit;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;
using GoFigure.App.ViewModels.Menu;
using Caliburn.Micro;
using System.Threading.Tasks;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Solution;

namespace GoFigure.Tests.ViewModels
{
  public class GameMenuViewModelTests : ViewModelTestsBase<GameMenuViewModel>
  {
    private IWindowManager _windowManager;
    private IMessageBoxManager _messageBoxManager;
    private ISoundEffectPlayer _soundEffectPlayer;
    private ISolutionGenerator _solutionGenerator;
    private IHighScoresScreenViewModel _highScoresViewModel;
    private GameSettings _gameSettings;

    public GameMenuViewModelTests()
    {
      _eventAggregator = A.Fake<IEventAggregatorWrapper>();
      _windowManager = A.Fake<IWindowManager>();
      _messageBoxManager = A.Fake<IMessageBoxManager>();
      _soundEffectPlayer = A.Fake<ISoundEffectPlayer>();
      _solutionGenerator = A.Fake<ISolutionGenerator>();
      _highScoresViewModel = A.Fake<IHighScoresScreenViewModel>();
      _gameSettings = new GameSettings();

      _viewModel = new GameMenuViewModel(
        _eventAggregator,
        _windowManager,
        _messageBoxManager,
        _soundEffectPlayer,
        _solutionGenerator,
        _highScoresViewModel,
        _gameSettings
      );

      _testUiComponent = new DependencyObject();
    }

    [Fact]
    public async Task When_StartNewGame_Is_Called_Then_NewGameStarted_Event_Is_Published()
    {
      await _viewModel.StartNewGame();

      AssertMessageWasPublished<NewGameStartedMessage>();
    }

    /*
      Level = 1,
      Solution = _generator.Generate(1, -1)
     */

    [Fact]
    public async Task When_StartNewGame_Is_Called_Then_Published_NewGameStarted_Event_Has_Correct_Level()
    {
      await _viewModel.StartNewGame();

      PublishMessageCallMatching<NewGameStartedMessage>(m => m.Level == 1)
        .MustHaveHappened();
    }

    [Fact]
    public async Task When_StartNewGame_Is_Called_Then_Solution_Generator_Is_Called()
    {
      await _viewModel.StartNewGame();

      A.CallTo(() => _solutionGenerator.Generate(A<int>._, A<int>._))
        .MustHaveHappened();
    }

    [Fact]
    public async Task When_StartNewGame_Is_Called_Then_NewGameStarted_Event_Contains_Generated_Solution()
    {
      var solution = new SolutionPlan();

      A.CallTo(() => _solutionGenerator.Generate(A<int>._, A<int>._))
        .Returns(solution);

      await _viewModel.StartNewGame();

      PublishMessageCallMatching<NewGameStartedMessage>(m => m.Solution == solution)
        .MustHaveHappened();
    }
  }
}
