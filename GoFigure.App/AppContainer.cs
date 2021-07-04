using Caliburn.Micro;
using SimpleInjector;
using System.Linq;
using System.Reflection;

namespace GoFigure.App
{
    class AppContainer : Container
    {
        private static readonly string ViewModelAssemblyName = $"{Assembly.GetExecutingAssembly().GetName().Name}.{nameof(ViewModels)}";

        public void Configure()
        {
            Register<IWindowManager, WindowManager>();
            Register<IEventAggregator, EventAggregator>();

            GetType().Assembly
                .GetTypes()
                .Where(t => t.FullName.StartsWith(ViewModelAssemblyName))
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList()
                .ForEach(Register);

            Verify();
        }
    }
}
