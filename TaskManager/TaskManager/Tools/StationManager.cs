using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using TaskManager.Models;

namespace TaskManager.Tools
{
    internal class StationManager
    {
        private static StationManager _instance;
        private MyProcess _selectedProcess;
        internal int i;
        #region Fields
        public static event Action StopThreads;
        private static List<MyProcess> _processList;
        #endregion

        #region Properties
        internal static List<MyProcess> ProcessList
        {
            get => _processList;
            set => _processList = value;
        }
        internal static StationManager Instance => _instance ??= new StationManager();
        internal MyProcess SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
            }
        }
        internal static int SortingParameter { get; set; }
        #endregion

        internal static void Initialize()
        {
            Instance.i = 0;
            SortingParameter = 1;
            ProcessList = new List<MyProcess>();
        }

        internal void DeleteProcess()
        {
            _processList.Remove(SelectedProcess);
        }

        internal static void UpdateProcessList()
        {
            if(Instance.i!=0)
                LoaderManager.Instance.ShowLoader();
            _processList.Clear();
            AddMissingProcesses();
            SortProcessList();
            if (Instance.i != 0)
                LoaderManager.Instance.HideLoader();
        }

        internal static void SortProcessList()
        {
            switch (SortingParameter)
            {
                case 0:
                    _processList = (from u in _processList
                                    orderby u.GetId
                                    select u).ToList();
                    break;

                case 1:
                    _processList = (from u in _processList
                                    orderby u.GetName
                                    select u).ToList();
                    break;
                case 2:
                    _processList = (from u in _processList
                                    orderby u.IsActive
                                    select u).ToList();
                    break;
                case 3:
                    _processList = (from u in _processList
                                    orderby u.GetCpu descending
                                    select u).ToList();
                    break;
                case 4:
                    _processList = (from u in _processList
                                    orderby u.GetRam descending
                                    select u).ToList();
                    break;
                case 5:
                    _processList = (from u in _processList
                                    orderby u.GetThreadsNum descending
                                    select u).ToList();
                    break;
                case 6:
                    _processList = (from u in _processList
                                    orderby u.User descending
                                    select u).ToList();
                    break;
                case 7:
                    _processList = (from u in _processList
                                    orderby u.GetFilePath
                                    select u).ToList();
                    break;
                default:
                    _processList = (from u in _processList
                                    orderby u.GetStartDate descending
                                    select u).ToList();
                    break;
            }
        }

        private static void AddMissingProcesses()
        {
            foreach (var item in Process.GetProcesses())
            {
                if (item != null)
                {
                    //if (!FoundTheSameInProcessList(item.Id))
                        ProcessList.Add(new MyProcess(item));
                }
            }
        }

        private static bool FoundTheSameInProcessList(int processId)
        {
            foreach (var item in _processList)
            {
                if (processId == item.GetId)
                {
                    return true;
                }
            }
            return false;
        }



        internal static void CloseApp()
        {
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}
