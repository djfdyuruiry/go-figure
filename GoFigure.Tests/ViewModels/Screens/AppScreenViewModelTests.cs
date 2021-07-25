using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

using FakeItEasy;
using Xunit;

using GoFigure.App.Model.Settings;
using GoFigure.App.ViewModels.Interfaces;
using GoFigure.App.ViewModels.Screens;
using GoFigure.App.Utils.Interfaces;

using static GoFigure.Tests.TestUtils;

namespace GoFigure.Tests.ViewModels.Screens
{
  public class AppScreenViewModelTests : ViewModelTestsBase<AppScreenViewModel>
  {
    private IMenuBarViewModel _menu;
    private IGameSettingsStore _gameSettingsStore;

    public AppScreenViewModelTests() : base()
    {
      _menu = A.Fake<IMenuBarViewModel>();
      _gameSettingsStore = A.Fake<IGameSettingsStore>();

      _viewModel = new AppScreenViewModel(
        _eventAggregator,
        _gameSettingsStore,
        new GameSettings(),
        _menu,
        A.Fake<IGameViewModel>()
      );
    }

    [Fact]
    public void When_ViewModel_Is_Constructed_Then_It_Subscribes_To_Events() =>
      RunEventSubscribeTest();

    [Fact]
    public async Task When_F1Key_Is_Pressed_Then_ShowHelp_Is_Called() =>
      await RunKeyPressTest(Key.F1, () =>
        A.CallTo(() => _menu.ShowHelp())
          .MustHaveHappened()
      );

    [Fact]
    public async Task When_F2Key_Is_Pressed_Then_PublishNewGameMessage_Is_Called() =>
      await RunKeyPressTest(Key.F2, () =>
        A.CallTo(() => _menu.PublishNewGameMessage())
          .MustHaveHappened()
      );

    [Fact]
    public async Task When_F2Key_Is_Pressed_Then_PublishPauseOrResumeGameMessage_Is_Called() =>
      await RunKeyPressTest(Key.F3, () => 
        A.CallTo(() => _menu.PauseOrResumeGame())
          .MustHaveHappened()
      );

    [Fact]
    public async Task When_SaveCurrentSettings_Is_Called_Then_GameSettingsStore_Is_Called()
    {
      await _viewModel.SaveCurrentSettings();

      A.CallTo(() => _gameSettingsStore.Write(A<GameSettings>._))
        .MustHaveHappened();
    }

    private async Task RunKeyPressTest(Key key, Action test) =>
      await RunOnUiThread(() =>
      {
        _viewModel.KeyPressed
        (
          new KeyEventArgs
          (
            Keyboard.PrimaryDevice,
            new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero),
            0,
            key
          )
        ).Wait();

        test();
      });
  }
}
