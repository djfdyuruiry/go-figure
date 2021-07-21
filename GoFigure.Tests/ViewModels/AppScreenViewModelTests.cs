using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

using FakeItEasy;
using Xunit;

using GoFigure.App.Model.Settings;
using GoFigure.App.ViewModels;
using GoFigure.App.ViewModels.Interfaces;
using GoFigure.App.Utils.Interfaces;

namespace GoFigure.Tests.ViewModels
{
    public class AppScreenViewModelTests
    {
        private IMenuBarViewModel _menu;
        private IGameSettingsStore _gameSettingsStore;

        private AppScreenViewModel _viewModel;

        public AppScreenViewModelTests()
        {
            _menu = A.Fake<IMenuBarViewModel>();
            _gameSettingsStore = A.Fake<IGameSettingsStore>();

            _viewModel = new AppScreenViewModel(
                A.Fake<IEventAggregatorWrapper>(),
                _gameSettingsStore,
                new GameSettings(),
                _menu,
                A.Fake<IGameViewModel>()
            );
        }

        [Fact]
        public async Task When_F1Key_Is_Pressed_Then_ShowHelp_Is_Called() =>
            await RunKeyPressTest(Key.F1, () =>
            {
                A.CallTo(() => _menu.ShowHelp())
                    .MustHaveHappened();
            });

        [Fact]
        public async Task When_F2Key_Is_Pressed_Then_PublishNewGameMessage_Is_Called() =>
            await RunKeyPressTest(Key.F2, () =>
            {
                A.CallTo(() => _menu.PublishNewGameMessage())
                    .MustHaveHappened();
            });

        [Fact]
        public async Task When_F2Key_Is_Pressed_Then_PublishPauseOrResumeGameMessage_Is_Called() =>
            await RunKeyPressTest(Key.F3, () =>
            {
                A.CallTo(() => _menu.PublishPauseOrResumeGameMessage())
                    .MustHaveHappened();
            });

        [Fact]
        public void When_SaveCurrentSettings_Is_Called_Then_GameSettingsStore_Is_Called()
        {
            _viewModel.SaveCurrentSettings();

            A.CallTo(() => _gameSettingsStore.Write(A<GameSettings>._))
                .MustHaveHappened();
        }

        private async Task RunKeyPressTest(Key key, System.Action test)
        {

            var tcs = new TaskCompletionSource<bool>();
            var thread = new Thread(() =>
            {
                try
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
                    tcs.SetResult(true);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            await tcs.Task;
        }
    }
}
