using TaskManager.Navigation;
using TaskManager.ViewModels;

namespace TaskManager.Views
{
    /// <summary>
    /// Interaction logic for ModulesView.xaml
    /// </summary>
    public partial class ModulesView : INavigatable
    {
        public ModulesView() 
        {
            InitializeComponent();
            DataContext = new ModulesViewModel();
        }
    }
}
