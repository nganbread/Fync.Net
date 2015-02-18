using Fync.Client.Windows.ViewModels;

namespace Fync.Client.Windows.Views
{
    /// <summary>
    /// Interaction logic for TaskBar.xaml
    /// </summary>
    internal partial class TaskBarView
    {
        public TaskBarView(TaskBarViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
