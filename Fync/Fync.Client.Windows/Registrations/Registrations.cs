using System;
using System.Windows;
using Fync.Client.DataBase;
using Fync.Client.Windows.Services;
using Fync.Client.Windows.ViewModels;
using Fync.Client.Windows.Views;
using Fync.Client.Windows.Views.Windows;
using TinyIoC;

namespace Fync.Client.Windows.Registrations
{
    internal static class Registrations
    {
        public static void Register(TinyIoCContainer container)
        {
            RegisterViews(container);
            RegisterWindows(container);
            RegisterFactories(container);
            RegisterViewModels(container);
            RegisterServices(container);
        }

        private static void RegisterFactories(TinyIoCContainer container)
        {
            container.Register<Func<string, Window>>((c, overloads) => WindowManufacturer(c));
        }

        private static void RegisterViews(TinyIoCContainer container)
        {
            container.Register<TaskBarView>(Views.TaskBar);
        }

        private static void RegisterWindows(TinyIoCContainer container)
        {
            container.Register<Window, LoggingWindow>(Windows.Main);
        }

        private static Func<string, Window> WindowManufacturer(TinyIoCContainer tinyIoCContainer)
        {
            return tinyIoCContainer.Resolve<Window>;
        }

        private static void RegisterServices(TinyIoCContainer container)
        {
            container.Register<IWindowLauncher, WindowLauncher>().AsSingleton();
            container.Register<ILogger, Services.Logger>().AsSingleton();
            container.Register<IHashCache, LocalDatabaseHashCache>().AsSingleton();
            container.Register<IClientConfiguration, ClientConfiguration>().AsSingleton();
        }

        private static void RegisterViewModels(TinyIoCContainer container)
        {
            container.Register<TaskBarViewModel>();
        }
    }
}
