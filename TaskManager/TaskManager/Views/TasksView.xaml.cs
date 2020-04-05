using TaskManager.Navigation;
using TaskManager.ViewModels;

namespace TaskManager.Views
{
    /// <summary>
    /// Interaction logic for TasksView.xaml
    /// </summary>
    public partial class TasksView : INavigatable
    {
        public TasksView()
        {
            InitializeComponent();
            DataContext = new TasksViewModel();
        }
    }
}
