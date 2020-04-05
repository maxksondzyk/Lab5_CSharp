using System.ComponentModel;
using TaskManager.Tools;
using TaskManager.ViewModels;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            StationManager.Instance.Initialize();
            DataContext = new MainWindowViewModel();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StationManager.Instance.CloseApp();
        }
    }
}
