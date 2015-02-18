using System;
using System.Windows;

namespace Fync.Client.Windows.Services
{
    internal class WindowLauncher : IWindowLauncher
    {
        private readonly Func<string, Window> _windowManufacturer;

        public WindowLauncher(Func<string, Window> windowManufacturer)
        {
            _windowManufacturer = windowManufacturer;
        }

        public void Launch(string viewName)
        {
            Application.Current.MainWindow = _windowManufacturer(viewName);
            Application.Current.MainWindow.Show();
        }
    }
}
