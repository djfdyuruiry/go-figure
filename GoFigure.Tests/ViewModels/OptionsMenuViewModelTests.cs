using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FakeItEasy;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;
using GoFigure.App.ViewModels.Menu;
using Xunit;

namespace GoFigure.Tests.ViewModels
{
  public class OptionsMenuViewModelTests
  {
    private IEventAggregatorWrapper _eventAggregator;
    private IMessageBoxManager _messageBoxManager;
    private ISoundEffectPlayer _soundEffectPlayer;
    private ISolutionGenerator _solutionGenerator;
    private GameSettings _gameSettings;

    private OptionsMenuViewModel _viewModel;

    private DependencyObject _testUiComponent;

    public OptionsMenuViewModelTests()
    {
      _eventAggregator = A.Fake<IEventAggregatorWrapper>();
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

      _testUiComponent = new DependencyObject();
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
    public async Task When_ToggleSound_Is_Called_Then_Game_Settings_Event_Is_Published()
    {
      await _viewModel.ToggleSound();

      A.CallTo(() => 
        _eventAggregator.PublishOnCurrentThreadAsync(
          A<ZeroDataMessage>.That.IsEqualTo(ZeroDataMessage.GameSettingsChanged)
        )
      ).MustHaveHappened();
    }
  }
}
