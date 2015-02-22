using System.Threading.Tasks;
using System.Windows;
using Fync.Client.DataBase;
using Fync.Client.Windows.Views;
using TinyIoC;

namespace Fync.Client.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private TinyIoCContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _container = new TinyIoCContainer();
            Common.Registrations.Register(_container);
            Client.Registrations.Register(_container);
            Registrations.Register(_container);

            //Start
            Logger.Instance = _container.Resolve<ILogger>();
            _container.Resolve<TaskBarView>();
            Task.Run(() => _container.Resolve<ISyncEngine>().Start());
            Task.Run(() => _container.Resolve<IFileMonitor>().Monitor());
            Task.Run(() => _container.Resolve<IFolderMonitor>().Monitor());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _container.Resolve<IHashCache>().Dispose();
        }
    }
}
