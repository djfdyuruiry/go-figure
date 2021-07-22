using System;
using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;
using SimpleInjector;

using GoFigure.App.Utils;
using GoFigure.App.Utils.Interfaces;

namespace GoFigure.App
{
  public class AppContainer : Container
  {
    private static readonly string UtilsNamespace =
      $"{nameof(GoFigure)}.{nameof(GoFigure.App)}.{nameof(Utils)}";
    private static readonly string UtilsInterfaceNamespace =
      $"{UtilsNamespace}.{nameof(Utils.Interfaces)}";
    private static readonly string ViewModelNamespace =
      $"{nameof(GoFigure)}.{nameof(GoFigure.App)}.{nameof(ViewModels)}";
    private static readonly string ViewModelInterfaceNamespace =
      $"{ViewModelNamespace}.{nameof(ViewModels.Interfaces)}";
    private static readonly Type[] Types = 
      typeof(AppContainer).Assembly.GetTypes();

    public AppContainer()
    {
      Options.ResolveUnregisteredConcreteTypes = true;
      Options.DefaultLifestyle = Lifestyle.Transient;
    }

    public void Configure()
    {
      RegisterSingleton<IWindowManager, WindowManager>();
      RegisterSingleton<IEventAggregator, EventAggregator>();

      ConfigureGameSettings();

      RegisterUtils();
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

    private void RegisterUtils() =>
      GetInterfaceMappings(
        UtilsInterfaceNamespace,
        UtilsNamespace,
        interfaceFilter: t => t != typeof(IGameSettingsStore)
      ).Foreach(RegisterSingleton);

    private void RegisterViewModels() =>
      GetInterfaceMappings(
        ViewModelInterfaceNamespace,
        ViewModelNamespace,
        t => t.Namespace?.StartsWith(ViewModelNamespace) ?? false
          && ViewModelInterfaceNamespace != t.Namespace,
        t => t != typeof(IGameSettingsStore)
      ).Foreach(Register);

    private IDictionary<Type, Type> GetInterfaceMappings(
      string interfaceNamespace,
      string classNamespace,
      Func<Type, bool> classFilter = null,
      Func<Type, bool> interfaceFilter = null
    )
    {
      var classes = Types.Where(t =>
          !t.IsInterface
            && (classFilter?.Invoke(t) ?? classNamespace == t?.Namespace)
        ).ToList();

      return Types.Where(t =>
          interfaceNamespace == t?.Namespace
            && t.IsInterface
            && (interfaceFilter?.Invoke(t) ?? true)
        )
        .ToDictionary(
          interfaceType => interfaceType,
          interfaceType => classes.Where(u =>
            interfaceType.Name == $"I{u.Name}"
          ).FirstOrDefault()
        );
    }
  }
}
