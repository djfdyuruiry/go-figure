using System.IO;
using System.Text.Json;

using Caliburn.Micro;
using SimpleInjector;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;
using GoFigure.App.Properties;

using static GoFigure.App.Constants;
using GoFigure.App.ViewModels;

namespace GoFigure.App
{
    public class AppContainer : Container
    {
        public AppContainer()
        {
            Options.ResolveUnregisteredConcreteTypes = true;
            Options.DefaultLifestyle = Lifestyle.Transient;
        }

        public void Configure()
        {
            RegisterSingleton<IWindowManager, WindowManager>();
            RegisterSingleton<IEventAggregator, EventAggregator>();
            RegisterSingleton<IEventAggregatorWrapper, EventAggregatorWrapper>();

            RegisterSingleton<ICalculator, Calculator>();
            RegisterSingleton<ISolutionComputer, SolutionComputer>();
            RegisterSingleton<ISolutionGenerator, SolutionGenerator>();
            RegisterSingleton<IMessageBoxManager, MessageBoxManager>();
            RegisterSingleton<ISoundEffectPlayer, SoundEffectPlayer>();

            ConfigureGameSettings();

            Verify();
        }

        private void ConfigureGameSettings()
        {
            var settingsStore = new GameSettingsStore();
            var gameSettings = settingsStore.Read().Result;

            RegisterInstance<IGameSettingsStore>(settingsStore);
            RegisterInstance(gameSettings);
        }
    }
}
