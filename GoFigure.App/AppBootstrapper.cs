using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

using GoFigure.App.ViewModels;
using GoFigure.App.ViewModels.Menu;
using GoFigure.App.Views;

using Caliburn.Micro;

namespace GoFigure.App
{
    class AppBootstrapper : BootstrapperBase
    {
        private readonly AppContainer _appContainer;

        public AppBootstrapper()
        {
            _appContainer = new AppContainer();

            Initialize();
        }

        protected override IEnumerable<Assembly> SelectAssemblies() =>
            base.SelectAssemblies()
                .Append(typeof(AppScreenView).GetTypeInfo().Assembly)
                .Append(typeof(AppScreenViewModel).GetTypeInfo().Assembly)
                .Append(typeof(MenuBarViewModel).GetTypeInfo().Assembly);

        protected override void Configure() => 
            _appContainer.Configure();

        protected override object GetInstance(Type service, string key) =>
            _appContainer.GetInstance(service);

        protected override IEnumerable<object> GetAllInstances(Type serviceType) =>
            (_appContainer as IServiceProvider).GetService(
                typeof(IEnumerable<>).MakeGenericType(serviceType)
            ) as IEnumerable<object> ?? Enumerable.Empty<object>();

        protected override void BuildUp(object instance) => 
            _appContainer.GetRegistration(instance.GetType(), true)
                .Registration
                .InitializeInstance(instance);

        protected override void OnStartup(object sender, StartupEventArgs e) =>
            DisplayRootViewFor<AppScreenViewModel>();
    }
}
