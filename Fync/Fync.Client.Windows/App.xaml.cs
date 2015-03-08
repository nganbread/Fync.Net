using System.Threading.Tasks;
using System.Windows;
using Fync.Client.Hash;
using Fync.Client.Monitor;
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

            var a = _container.CanResolve<ISyncEngine>();
            var syncEngine = _container.Resolve<ISyncEngine>();
            var fileMonitor = _container.Resolve<IFileMonitor>();
            var folderMonitor = _container.Resolve<IFolderMonitor>();

            Task.Factory.StartNew(syncEngine.Start).ContinueWith(t =>
            {
                Task.Run(() =>
                {
                    fileMonitor.Monitor();
                });
                Task.Run(() =>
                {
                    folderMonitor.Monitor();
                });
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _container.Resolve<IHashCache>().Dispose();
        }
    }
}
