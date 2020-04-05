using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using TaskManager.Models;
using TaskManager.Navigation;
using TaskManager.Tools;

namespace TaskManager.ViewModels
{
    class ModulesViewModel : BaseViewModel
    {
        private RelayCommand<object> _return;
        private ObservableCollection<MyModule> _modules;

        public string ProcessName
        {
            get => StationManager.Instance.SelectedProcess.GetName;
        }

        public ObservableCollection<MyModule> Modules
        {
            get
            {

                return _modules;

            }
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
            StationManager.Instance.i = 1;
        }

        public Action CloseAction { get; set; }

        internal ModulesViewModel()
        {
            Modules = new ObservableCollection<MyModule>();
            ObservableCollection<MyModule> tmp = new ObservableCollection<MyModule>();
            int id = StationManager.Instance.SelectedProcess.GetId;
            foreach (ProcessModule module in StationManager.Instance.SelectedProcess.Modules)
            {
                tmp.Add(new MyModule(module));
            }
            Modules = tmp;
        }
    }
}
