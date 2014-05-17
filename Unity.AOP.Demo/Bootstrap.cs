using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Windows;
using Unity.AOP.Demo.Services;

namespace Unity.AOP.Demo
{
    public class Bootstrap : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            new UnityAopModule().Initialize();
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureContainer()
        {
            Container.LoadConfiguration();
            base.ConfigureContainer();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = new ModuleCatalog();
            catalog.AddModule(typeof(UnityAopModule));
            catalog.Initialize();
            
            return catalog;
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
