using Caliburn.Micro;
using SimpleInjector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public void Configure()
        {
            RegisterSingleton<IWindowManager, WindowManager>();
            RegisterSingleton<IEventAggregator, EventAggregator>();

            GetType().Assembly
                .GetTypes()
                .Where(t => NamespacesToAutoRegister.Any(t.FullName.StartsWith))
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList()
                .ForEach(Register);

            Verify();
        }
    }
}
