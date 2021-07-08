using Caliburn.Micro;
using SimpleInjector;

using GoFigure.App.Model;

namespace GoFigure.App
{
    class AppContainer : Container
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
            RegisterSingleton<GameSettings>();

            Verify();
        }
    }
}
