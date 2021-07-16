using Caliburn.Micro;
using SimpleInjector;

using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;

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

            RegisterSingleton<ICalculator, Calculator>();
            RegisterSingleton<ISolutionComputer, SolutionComputer>();
            RegisterSingleton<ISolutionGenerator, SolutionGenerator>();
            RegisterSingleton<IMessageBoxManager, MessageBoxManager>();
            RegisterSingleton<ISoundEffectPlayer, SoundEffectPlayer>();

            RegisterInstance(
                new GameSettings
                {
                    SoundEnabled = true
                }
            );

            Verify();
        }
    }
}
