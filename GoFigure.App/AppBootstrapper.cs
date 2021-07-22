using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Caliburn.Micro;

using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App
{
  public class AppBootstrapper : BootstrapperBase
  {
    private readonly AppContainer _appContainer;

    public AppBootstrapper()
    {
      _appContainer = new AppContainer();

      Initialize();
    }

    protected override void Configure() => 
      _appContainer.Configure();

    protected override object GetInstance(Type service, string key) =>
      _appContainer.GetInstance(service);

    protected override IEnumerable<object> GetAllInstances(Type serviceType) =>
      (_appContainer as IServiceProvider).GetService(
        typeof(IEnumerable<>).MakeGenericType(serviceType)
      ) as IEnumerable<object> ?? Enumerable.Empty<object>();

    protected override void OnStartup(object sender, StartupEventArgs e) =>
      DisplayRootViewFor<IAppScreenViewModel>();
  }
}
