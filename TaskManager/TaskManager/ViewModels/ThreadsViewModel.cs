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
    internal class ThreadsViewModel : BaseViewModel
    {

        private RelayCommand<object> _return;
        private ObservableCollection<MyThread> _threads;

        public string ProcessName
        {
            get => StationManager.Instance.SelectedProcess.GetName;
        }
        public ObservableCollection<MyThread> Threads
        {
            get
            {

                return _threads;

            }
            private set
            {
                _threads = value;
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
        public Action CloseAction { get; set; }

        internal ThreadsViewModel()
        {
            Threads = new ObservableCollection<MyThread>();
            ObservableCollection<MyThread> tmp = new ObservableCollection<MyThread>();
            int id = StationManager.Instance.SelectedProcess.GetId;
            foreach (ProcessThread thread in StationManager.Instance.SelectedProcess.ThreadsCollection)
            {
                tmp.Add(new MyThread(thread));
            }
            Threads = tmp;
        }
    }
}