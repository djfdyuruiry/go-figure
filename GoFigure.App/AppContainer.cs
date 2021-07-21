using System.Linq;

using Caliburn.Micro;
using SimpleInjector;

using GoFigure.App.Utils;

namespace GoFigure.App
{
    public class AppContainer : Container
    {
        private static readonly string ViewModelNamespace =
            $"{nameof(GoFigure)}.{nameof(GoFigure.App)}.{nameof(ViewModels)}";
        private static readonly string ViewModelInterfaceNamespace =
            $"{ViewModelNamespace}.{nameof(ViewModels.Interfaces)}";

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

            ConfigureGameSettings();

            RegisterViewModels();

            Verify();
        }

        private void ConfigureGameSettings()
        {
            var settingsStore = new GameSettingsStore();
            var gameSettings = settingsStore.Read().Result;

            RegisterInstance<IGameSettingsStore>(settingsStore);
            RegisterInstance(gameSettings);
        }

        private void RegisterViewModels()
        {
            var types = GetType().Assembly
                .GetTypes();

            var viewModels =
                types.Where(t =>
                        !(t.Namespace is null)
                        && t.Namespace.StartsWith(ViewModelNamespace)
                        && t.Namespace != ViewModelInterfaceNamespace
                    )
                    .ToList();

            types.Where(t => t.Namespace == ViewModelInterfaceNamespace)
                .Select(interfaceType =>
                    (
                        interfaceType: interfaceType,
                        viewModelType: viewModels.Where(vm =>
                            interfaceType.Name == $"I{vm.Name}"
                        ).FirstOrDefault()
                    )
                )
                .ToList()
                .ForEach(viewModelInfo =>
                    Register(viewModelInfo.interfaceType, viewModelInfo.viewModelType)
                );
        }
    }
}
