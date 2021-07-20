using System;
using System.Windows.Input;
using System.Windows.Interop;

using Caliburn.Micro;
using FakeItEasy;
using Xunit;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;
using GoFigure.App.ViewModels;
using GoFigure.App.ViewModels.Menu;
using System.Threading.Tasks;
using System.Threading;
using GoFigure.App.Model.Messages;

namespace GoFigure.Tests.ViewModels
{
    public class AppScreenViewModelTests
    {
        private IEventAggregatorWrapper _eventAggregator;

        private AppScreenViewModel _viewModel;

        public AppScreenViewModelTests()
        {
            _eventAggregator = A.Fake<IEventAggregatorWrapper>();

            _viewModel = new AppScreenViewModel(
                _eventAggregator,
                A.Fake<IGameSettingsStore>(),
                new GameSettings(),
                A.Fake<MenuBarViewModel>(),
                A.Fake<GameViewModel>()
            );
        }

        [Fact]
        public async Task When_F2Key_Is_Pressed_Then_NewGameMessage_Is_Published()
        {
            await RunKeyPressTest(Key.F2, () =>
            {
                A.CallTo(() =>
                    _eventAggregator.PublishOnCurrentThreadAsync(A<NewGameStartedMessage>._)
                ).MustHaveHappened();
            });
        }


        private async Task RunKeyPressTest(Key key, System.Action test) =>
            await RunOnStaThread(() =>
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
                );

                test();
            });

        private async Task RunOnStaThread(System.Action test)
        {
            var tcs = new TaskCompletionSource<bool>();
            var thread = new Thread(() =>
            {
                try
                {
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
