using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using TaskManager.Models;
using TaskManager.Navigation;
using TaskManager.Tools;

namespace TaskManager.ViewModels
{
    internal class TasksViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<MyThread> _threads;
        private ObservableCollection<MyModule> _modules;
        private ObservableCollection<MyProcess> _processes;
        private Thread _workingThread;
        private Thread _processThread;
        private INavigatable _content;
        private readonly CancellationToken _token;
        private readonly CancellationTokenSource _tokenSource;

        #region Commands
        private RelayCommand<object> _openFolder;
        private RelayCommand<object> _showThreads;
        private RelayCommand<object> _showModules;
        private RelayCommand<object> _sortById;
        private RelayCommand<object> _sortByName;
        private RelayCommand<object> _sortByIsActive;
        private RelayCommand<object> _sortByCpu;
        private RelayCommand<object> _sortByRam;
        private RelayCommand<object> _sortByThreadsNumber;
        private RelayCommand<object> _sortByUser;
        private RelayCommand<object> _sortByFilepath;
        private RelayCommand<object> _sortByStartDate;
        private RelayCommand<object> _endTask;

        #endregion
        #endregion

        #region Properties

        public ObservableCollection<MyThread> Threads
        {
            get => _threads;
            private set
            {
                _threads = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MyModule> Modules
        {
            get => _modules;
            private set
            {
                _modules = value;
                OnPropertyChanged();
            }
        }
        public MyProcess SelectedProcess
        {
            get => StationManager.Instance.SelectedProcess;
            set
            {
                StationManager.Instance.SelectedProcess = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MyProcess> Processes
        {
            get => _processes;
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }
        #region Commands
        public RelayCommand<object> SortById
        {
            get
            {
                return _sortById ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 0));
            }
        }
        public RelayCommand<object> SortByName
        {
            get
            {
                return _sortByName ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 1));
            }
        }
        public RelayCommand<object> SortByIsActive
        {
            get
            {
                return _sortByIsActive ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 2));
            }
        }
        public RelayCommand<object> SortByCpu
        {
            get
            {
                return _sortByCpu ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 3));
            }
        }
        public RelayCommand<object> SortByRam
        {
            get
            {
                return _sortByRam ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 4));
            }
        }
        public RelayCommand<object> SortByThreadsNumber
        {
            get
            {
                return _sortByThreadsNumber ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 5));
            }
        }
        public RelayCommand<object> SortByUser
        {
            get
            {
                return _sortByUser ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 6));
            }
        }
        public RelayCommand<object> SortByFilepath
        {
            get
            {
                return _sortByFilepath ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 7));
            }
        }
        public RelayCommand<object> SortByStartDate
        {
            get
            {
                return _sortByStartDate ??= new RelayCommand<object>(o =>
                    SortImplementation(o, 8));
            }
        }
        public RelayCommand<object> ShowThreads
        {
            get
            {
                return _showThreads ??= new RelayCommand<object>(
                    ShowThreadsImplementation, o => CanExecuteCommand());
            }
        }
        public RelayCommand<object> OpenFolder
        {
            get
            {
                return _openFolder ??= new RelayCommand<object>(
                    OpenFolderImplementation, o => CanExecuteCommand());
            }
        }
        public RelayCommand<object> ShowModules
        {
            get
            {
                return _showModules ??= new RelayCommand<object>(
                    ShowModulesImplementation, o => CanExecuteCommand());
            }
        }
        public RelayCommand<object> EndTask
        {
            get
            {
                return _endTask ??= new RelayCommand<object>(
                    EndTaskImplementation, o => CanExecuteCommand());
            }
        }

        #endregion



        #endregion


        private void OpenFolderImplementation(object obj)
        {
            try
            {
                Process.Start("explorer.exe",
                    SelectedProcess.GetFilePath.Substring(0,
                        SelectedProcess.GetFilePath.LastIndexOf("\\", StringComparison.Ordinal)));
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while accessing processing data");
            }
        }
        private void ShowThreadsImplementation(object o)
        {
            try
            {
                StationManager.Instance.i = 0;
                Threads = new ObservableCollection<MyThread>();
                ObservableCollection<MyThread> tmp = new ObservableCollection<MyThread>();
                int id = SelectedProcess.GetId;
                foreach (ProcessThread thread in SelectedProcess.ThreadsCollection)
                {
                    tmp.Add(new MyThread(thread));
                }

                Threads = tmp;
                NavigationManager.Instance.Navigate(ViewType.Threads);
            }
            catch (Exception e)
            {
                MessageBox.Show("Access Denied");
            }
        }
        private void ShowModulesImplementation(object obj)
        {
            try
            {
                StationManager.Instance.i = 0;
                Modules = new ObservableCollection<MyModule>();
                ObservableCollection<MyModule> tmp = new ObservableCollection<MyModule>();
                int id = SelectedProcess.GetId;
                foreach (ProcessModule module in SelectedProcess.Modules)
                {
                    tmp.Add(new MyModule(module));
                }

                Modules = tmp;
                NavigationManager.Instance.Navigate(ViewType.Modules);
            }
            catch (Exception e)
            {
                MessageBox.Show("Access Denied");
            }
        }
        private async void EndTaskImplementation(object obj)
        {
            await Task.Run(() => {
                if (SelectedProcess.checkAvailability())
                {
                    SelectedProcess?.GetProcess?.Kill();
                    StationManager.Instance.DeleteProcess();
                    StationManager.Instance.i = 1;
                    StationManager.Instance.UpdateProcessList();
                    SelectedProcess = null;
                    Processes = new ObservableCollection<MyProcess>(StationManager.Instance.ProcessList);
                }
                else
                {
                    MessageBox.Show("Have no access");
                }
            }, _token);
        }
        private async void SortImplementation(object obj, int param)
        {
            await Task.Run(() =>
            {
                try
                {
                    StationManager.Instance.SortingParameter = param;
                    StationManager.Instance.i = 0;
                    StationManager.Instance.SortProcessList();
                    Processes = new ObservableCollection<MyProcess>(StationManager.Instance.ProcessList);
                    StationManager.Instance.i = 1;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error occurred while accessing process data");
                    StationManager.Instance.i = 1;
                }
            }, _token);
        }

        internal TasksViewModel()
        {
            _processes = new ObservableCollection<MyProcess>(StationManager.Instance.ProcessList);
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _workingThread = new Thread(WorkingThreadProcess);
            _workingThread.Start();
            StationManager.Instance.i = 1;
           // _processThread = new Thread(ProcessThreadProcess);
            //_processThread.Start();
        }

        private void ProcessThreadProcess()
        {
            while (!_token.IsCancellationRequested)
            {
                foreach (var p in Processes)
                {
                    p.Update();
                }
              
                if (_token.IsCancellationRequested)
                    break;
            }
            StationManager.StopThreads += StopWorkingThread;
        }
        private void WorkingThreadProcess()
        {

            while (!_token.IsCancellationRequested)
            {
                var temp = -1;
                if (SelectedProcess != null)
                {
                    temp = SelectedProcess.GetId;
                }

                //StationManager.Instance.i = 1;
                StationManager.Instance.UpdateProcessList();
                Processes = new ObservableCollection<MyProcess>(StationManager.Instance.ProcessList);

                foreach (var p in Processes)
                {
                    if (p.GetId != temp) continue;
                   SelectedProcess = p;
                    break;
                }
                Thread.Sleep(5000);

                if (_token.IsCancellationRequested)
                    break;
            }
            StationManager.StopThreads += StopWorkingThread;
        }
        internal void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(1000);
            _workingThread.Abort();
            _workingThread = null;
        }

        private bool CanExecuteCommand()
        {
            return SelectedProcess != null;
        }
       
        public INavigatable Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
