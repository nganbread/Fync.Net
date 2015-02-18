using System.Windows;
using Fync.Client.Windows.ViewModels.Windows;

namespace Fync.Client.Windows.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class LoggingWindow : Window
    {
        public LoggingWindow(LoggingWindowModel model)
        {
            DataContext = model;
            InitializeComponent();
        }
    }
}
