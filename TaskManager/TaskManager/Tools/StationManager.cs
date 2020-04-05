using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaskManager.Models;

namespace TaskManager.Tools
{
    internal class StationManager
    {

        #region Fields

      
        private static StationManager _instance;
        internal int I;
        public static event Action StopThreads;
        private static List<MyProcess> _processList;
        #endregion

        #region Properties
        internal List<MyProcess> ProcessList
        {
            get => _processList;
            set => _processList = value;
        }
        internal static StationManager Instance => _instance ??= new StationManager();
        internal MyProcess SelectedProcess { get; set; }

        internal int SortingParameter { get; set; }
        #endregion

        internal void Initialize()
        {
            Instance.I = 0;
            SortingParameter = 1;
            ProcessList = new List<MyProcess>();
        }

        internal void DeleteProcess()
        {
            _processList.Remove(SelectedProcess);
        }

        private void Clear()
        {
            try
            {

                foreach (var item in ProcessList)
                {
                    if (!FoundTheSameInProcessSys(item.GetId))
                    {
                        ProcessList.Remove(item);
                    }
                    item.GetRam = item.GetProcess.WorkingSet64 / 1024 / 1024;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        internal void UpdateProcessList()
        {
            if (I == 0) 
                LoaderManager.Instance.ShowLoader();
            else
                Clear();
            AddMissingProcesses();
            SortProcessList();


            if (I != 0) return;
            LoaderManager.Instance.HideLoader();
            I = 1;
        }

       

        internal void SortProcessList()
        {
            _processList = SortingParameter switch
            {
                0 => (from u in _processList orderby u.GetId select u).ToList(),
                1 => (from u in _processList orderby u.GetName select u).ToList(),
                2 => (from u in _processList orderby u.IsActive select u).ToList(),
                3 => (from u in _processList orderby u.GetCpu descending select u).ToList(),
                4 => (from u in _processList orderby u.GetRam descending select u).ToList(),
                5 => (from u in _processList orderby u.GetThreadsNum descending select u).ToList(),
                6 => (from u in _processList orderby u.User descending select u).ToList(),
                7 => (from u in _processList orderby u.GetFilePath select u).ToList(),
                _ => (from u in _processList orderby u.GetStartDate descending select u).ToList()
            };
        }

        private void AddMissingProcesses()
        {
            if (Instance.SelectedProcess != null)
            {
                var temp = Instance.SelectedProcess.GetId;
                foreach (var item in Process.GetProcesses())
                {
                    if (item == null) continue;
                    if (!FoundTheSameInProcessList(item.Id))
                        ProcessList.Add(new MyProcess(item));
                    if (item.Id == temp)
                        Instance.SelectedProcess = new MyProcess(item);
                }
            }
            else
            {
                foreach (var item in Process.GetProcesses())
                {
                    if (item == null) continue;
                    if (!FoundTheSameInProcessList(item.Id))
                        ProcessList.Add(new MyProcess(item));
                }
            }
        }

        private bool FoundTheSameInProcessList(int processId)
        {
            return ProcessList.Any(item => processId == item.GetId);
        }
        private bool FoundTheSameInProcessSys(int processId)
        {
            return Process.GetProcesses().Any(item => processId == item.Id);
        }

        internal void CloseApp()
        {
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}
