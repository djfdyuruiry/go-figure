using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Caliburn.Micro;
using SimpleInjector;

using GoFigure.App.Model;

namespace GoFigure.App
{
    class AppContainer : Container
    {
        private static readonly string RootAssemblyName =
            Assembly.GetExecutingAssembly()
                .GetName()
                .Name;

        private static readonly IList<string> NamespacesToAutoRegister =
            new List<string>
            {
                $"{RootAssemblyName}.{nameof(ViewModels)}",
                $"{RootAssemblyName}.{nameof(ViewModels)}.{nameof(ViewModels.Menu)}",
                $"{RootAssemblyName}.{nameof(Utils)}"
            };

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
