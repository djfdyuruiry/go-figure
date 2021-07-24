using System.Windows;

using FakeItEasy;
using Xunit;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;
using GoFigure.App.ViewModels.Menu;
using System.Threading.Tasks;

using Caliburn.Micro;

using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Solution;
using GoFigure.App.Utils;

using System.Threading;
using System.Collections.Generic;

namespace GoFigure.Tests.ViewModels
{
  public class GameMenuViewModelTests : ViewModelTestsBase<GameMenuViewModel>
  {
    private IWindowManager _windowManager;
    private IMessageBoxManager _messageBoxManager;
    private ISoundEffectPlayer _soundEffectPlayer;
    private ISolutionGenerator _solutionGenerator;
    private IApplicationManager _appManager;
    private IHighScoresScreenViewModel _highScoresViewModel;
    private GameSettings _gameSettings;

    public GameMenuViewModelTests()
    {
      _eventAggregator = A.Fake<IEventAggregatorWrapper>();
      _windowManager = A.Fake<IWindowManager>();
      _messageBoxManager = A.Fake<IMessageBoxManager>();
      _soundEffectPlayer = A.Fake<ISoundEffectPlayer>();
      _solutionGenerator = A.Fake<ISolutionGenerator>();
      _appManager = A.Fake<IApplicationManager>();
      _highScoresViewModel = A.Fake<IHighScoresScreenViewModel>();
      _gameSettings = new GameSettings();

      _viewModel = new GameMenuViewModel(
        _eventAggregator,
        _windowManager,
        _messageBoxManager,
        _soundEffectPlayer,
        _solutionGenerator,
        _appManager,
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

      A.CallTo(() => 
        _solutionGenerator.Generate
        (
          A<int>.That.IsEqualTo(1), 
          A<int>.That.IsEqualTo(-1)
        )
      ).MustHaveHappened();
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

    [Fact]
    public async Task When_PauseOrResumeGame_Is_Called_And_Game_Is_Not_Paused_Then_PauseGame_Event_Is_Published()
    {
      await _viewModel.PauseOrResumeGame();

      AssertMessageWasPublished(ZeroDataMessage.PauseGame);
    }

    [Fact]
    public async Task When_PauseOrResumeGame_Is_Called_And_Game_Is_Paused_Then_ResumeGame_Event_Is_Published()
    {
      await _viewModel.HandleAsync(ZeroDataMessage.PauseGame, new CancellationToken());

      await _viewModel.PauseOrResumeGame();

      AssertMessageWasPublished(ZeroDataMessage.ResumeGame);
    }

    [Fact]
    public async Task When_ShowHighScores_Is_Called_Then_Window_Manager_Is_Called_With_High_Scores_View_Model()
    {
      await _viewModel.ShowHighScores();

      A.CallTo(() => 
        _windowManager.ShowWindowAsync(
          A<IHighScoresScreenViewModel>.That.IsEqualTo(_highScoresViewModel),
          A<object>._,
          A<IDictionary<string, object>>._
        )
      ).MustHaveHappened();
    }
    
    [Fact]
    public void When_CloseApp_Is_Called_Then_App_Manager_Is_Called()
    {
      _viewModel.CloseApp();

      A.CallTo(() => _appManager.Shutdown())
        .MustHaveHappened();
    }

    [Fact]
    public async Task When_NewGameStarted_Event_Is_Received_Then_CanPause_Is_Set_To_True()
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage(), new CancellationToken());

      Assert.True(_viewModel.CanPause);
    }

  }
}
