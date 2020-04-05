using System.Collections.ObjectModel;
using System.Diagnostics;
using TaskManager.Models;
using TaskManager.Navigation;
using TaskManager.Tools;

namespace TaskManager.ViewModels
{
    internal class ModulesViewModel : BaseViewModel
    {
        private RelayCommand<object> _return;
        private ObservableCollection<MyModule> _modules;

        public string ProcessName => StationManager.Instance.SelectedProcess.GetName;

        public ObservableCollection<MyModule> Modules
        {
            get => _modules;
            private set
            {
                _modules = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand<object> Return
        {
            get
            {
                return _return ??= new RelayCommand<object>(
                    ReturnImplementation);
            }
        }
        private void ReturnImplementation(object obj)
        {
            NavigationManager.Instance.Navigate(ViewType.Tasks);
        }

        internal ModulesViewModel()
        {
            Modules = new ObservableCollection<MyModule>();
            var tmp = new ObservableCollection<MyModule>();
            foreach (ProcessModule module in StationManager.Instance.SelectedProcess.Modules)
            {
                tmp.Add(new MyModule(module));
            }
            Modules = tmp;
        }
    }
}
