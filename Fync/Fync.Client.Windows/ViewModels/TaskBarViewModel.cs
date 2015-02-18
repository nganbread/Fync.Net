using System.Windows;
using System.Windows.Input;
using Fync.Client.Windows.Components;
using Fync.Client.Windows.Services;

namespace Fync.Client.Windows.ViewModels
{
    internal class TaskBarViewModel
    {
        private readonly IWindowLauncher _windowLauncher;
        private readonly ICommand _showLoggingWindow;
        private readonly ICommand _exitCommand;

        public TaskBarViewModel(IWindowLauncher windowLauncher)
        {
            _windowLauncher = windowLauncher;
            _showLoggingWindow = new Command(ShowWindow);
            _exitCommand = new Command(ExitApplication);
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private void ShowWindow()
        {
            _windowLauncher.Launch(Registrations.Windows.Main);
        }

        public ICommand ShowLoggingWindow
        {
            get { return _showLoggingWindow; }
        }

        public ICommand ExitCommand
        {
            get { return _exitCommand; }
        }
    }
}
