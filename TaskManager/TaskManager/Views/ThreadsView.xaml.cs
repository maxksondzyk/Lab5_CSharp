using TaskManager.Navigation;
using TaskManager.ViewModels;

namespace TaskManager.Views
{
    /// <summary>
    /// Interaction logic for ThreadsView.xaml
    /// </summary>
    public partial class ThreadsView : INavigatable
    {
        public ThreadsView()
        {
            InitializeComponent();
            DataContext = new ThreadsViewModel();
        }
    }
}
