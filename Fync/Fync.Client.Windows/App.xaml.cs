using System;
using System.Threading.Tasks;
using System.Windows;
using Fync.Client.Hash;
using Fync.Client.Windows.Views;
using Fync.Common;
using Fync.Common.Common;
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

            try
            {
                var syncEngine = _container.Resolve<ISyncEngine>();
                Task.Run(() =>
                {
                    try
                    {
                        syncEngine.Start();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                });

            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex);   
            }

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _container.Resolve<IHashCache>().Dispose();
        }
    }
}
