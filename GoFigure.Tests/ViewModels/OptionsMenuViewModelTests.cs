using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using FakeItEasy;
using Xunit;

using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Menu;

namespace GoFigure.Tests.ViewModels
{
  public class OptionsMenuViewModelTests : ViewModelTestsBase<OptionsMenuViewModel>
  {
    private IMessageBoxManager _messageBoxManager;
    private ISoundEffectPlayer _soundEffectPlayer;
    private ISolutionGenerator _solutionGenerator;
    private GameSettings _gameSettings;

    public OptionsMenuViewModelTests() : base()
    {
      _messageBoxManager = A.Fake<IMessageBoxManager>();
      _soundEffectPlayer = A.Fake<ISoundEffectPlayer>();
      _solutionGenerator = A.Fake<ISolutionGenerator>();
      _gameSettings = new GameSettings();

      _viewModel = new OptionsMenuViewModel(
        _eventAggregator,
        _messageBoxManager,
        _soundEffectPlayer,
        _solutionGenerator,
        _gameSettings
      );
    }

    [Fact]
    public void When_SoundEnabled_Read_Then_Value_Matches_Game_Settings()
    {
      _gameSettings.SoundEnabled = true;

      Assert.True(_viewModel.SoundEnabled);
    }

    [Fact]
    public void When_OperatorPrecedence_Read_Then_Value_Matches_Game_Settings()
    {
      _gameSettings.UseOperatorPrecedence = true;

      Assert.True(_viewModel.OperatorPrecedence);
    }

    [Fact]
    public void When_LeftToRightPrecedence_Read_Then_Value_Matches_Game_Settings()
    {
      _gameSettings.UseOperatorPrecedence = true;

      Assert.False(_viewModel.LeftToRightPrecedence);
    }

    [Fact]
    public async Task When_ToggleSound_Is_Called_Then_Game_Settings_Value_Is_Toggled()
    {
      await _viewModel.ToggleSound();

      Assert.True(_gameSettings.SoundEnabled);
    }

    [Fact]
    public async Task When_ToggleSound_Is_Called_Then_Sound_Effect_Player_Value_Matches_Game_Settings()
    {
      await _viewModel.ToggleSound();

      Assert.True(_soundEffectPlayer.Enabled);
    }

    [Fact]
    public async Task When_ToggleSound_Is_Called_Then_GameSettingsChanged_Event_Is_Published()
    {
      await _viewModel.ToggleSound();

      AssertMessageWasPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Fact]
    public async Task When_ShowSolutionHint_Is_Called_Then_ShowSolutionHint_Event_Is_Published()
    {
      await _viewModel.ShowSolutionHint();

      AssertMessageWasPublished(ZeroDataMessage.ShowSolutionHint);
    }

    [Fact]
    public async Task When_ClearSolution_Is_Called_Then_ClearSolution_Event_Is_Published()
    {
      await _viewModel.ClearSolution();

      AssertMessageWasPublished(ZeroDataMessage.ClearSolution);
    }

    [Fact]
    public async Task When_UseOperatorPrecedence_Is_Called_And_Game_Is_Not_Running_Then_GameSettingsChanged_Event_Is_Published()
    {
      await _viewModel.UseOperatorPrecedence(_testUiComponent);

      AssertMessageWasPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Fact]
    public async Task When_UseOperatorPrecedence_Is_Called_And_Game_Is_Running_And_User_Clicks_Ok_Then_GameSettingsChanged_Event_Is_Published()
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage(), new CancellationToken());

      A.CallTo(() =>
        _messageBoxManager.ShowOkCancel(A<DependencyObject>._, A<string>._)
      ).Returns(MessageBoxResult.OK);

      await _viewModel.UseOperatorPrecedence(_testUiComponent);

      AssertMessageWasPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Fact]
    public async Task When_UseOperatorPrecedence_Is_Called_And_Game_Is_Running_And_User_Clicks_Cancel_Then_GameSettingsChanged_Event_Is_Not_Published()
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage(), new CancellationToken());

      A.CallTo(() =>
        _messageBoxManager.ShowOkCancel(A<DependencyObject>._, A<string>._)
      ).Returns(MessageBoxResult.Cancel);

      await _viewModel.UseOperatorPrecedence(_testUiComponent);

      AssertMessageWasNotPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Fact]
    public async Task When_UseLeftToRightPrecedence_Is_Called_And_Game_Is_Not_Running_Then_GameSettingsChanged_Event_Is_Published()
    {
      await _viewModel.UseLeftToRightPrecedence(_testUiComponent);

      AssertMessageWasPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Fact]
    public async Task When_UseLeftToRightPrecedence_Is_Called_And_Game_Is_Running_And_User_Clicks_Ok_Then_GameSettingsChanged_Event_Is_Published()
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage(), new CancellationToken());

      A.CallTo(() =>
        _messageBoxManager.ShowOkCancel(A<DependencyObject>._, A<string>._)
      ).Returns(MessageBoxResult.OK);

      await _viewModel.UseLeftToRightPrecedence(_testUiComponent);

      AssertMessageWasPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Fact]
    public async Task When_UseLeftToRightPrecedence_Is_Called_And_Game_Is_Running_And_User_Clicks_Cancel_Then_GameSettingsChanged_Event_Is_Not_Published()
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage(), new CancellationToken());

      A.CallTo(() =>
        _messageBoxManager.ShowOkCancel(A<DependencyObject>._, A<string>._)
      ).Returns(MessageBoxResult.Cancel);

      await _viewModel.UseLeftToRightPrecedence(_testUiComponent);

      AssertMessageWasNotPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Fact]
    public async Task When_NewGameStarted_Message_Received_Then_HintEnabled_Is_Set_To_True()
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage(), new CancellationToken());

      Assert.True(_viewModel.HintEnabled);
    }

    [Fact]
    public async Task When_NoHintsLeft_Message_Received_Then_HintEnabled_Is_Set_To_False()
    {
      await _viewModel.HandleAsync(ZeroDataMessage.NoHintsLeft, new CancellationToken());

      Assert.False(_viewModel.HintEnabled);
    }
  }
}
