using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using TaskManager.Annotations;

namespace TaskManager.Models
{
    internal class MyProcess: INotifyPropertyChanged
    {
        #region Fields
        private readonly PerformanceCounter _perfCounter;
        private double _cpu;
        private float _ram;

        #endregion

        #region Properties

        public Process GetProcess { get ; set; }

        public string GetName => GetProcess.ProcessName;
        public int GetId => GetProcess.Id;
        public bool IsActive => GetProcess.Responding;

        public double GetCpu
        {
            get => _cpu;
            set { _cpu = value; OnPropertyChanged(); }
        }

        public float GetRam
        {
            get => _ram;
            set { _ram = value; OnPropertyChanged(); }
        }

        public ProcessThreadCollection Threads => GetProcess.Threads;
        public int GetThreadsNum => GetProcess.Threads.Count;

        public ProcessModuleCollection Modules => GetProcess.Modules;

        public ProcessThreadCollection ThreadsCollection => GetProcess.Threads;

        public string GetFilePath { get; set; }

        public string GetStartDate { get; set; }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        public string User { get; set; }

        #endregion

        internal void Update()
        {
            try
            {
                GetCpu = _perfCounter.NextValue() / Environment.ProcessorCount;
            }
            catch
            {
                GetCpu = 0;
            }
        }
        internal MyProcess(Process process)
        {
            GetProcess = process;

            var processHandle = IntPtr.Zero;
            
            try
            {
                _perfCounter = new PerformanceCounter("Process", "% Processor Time", GetName, true);
                _perfCounter.NextValue();
                GetRam = process.WorkingSet64/1024/1024;
                GetStartDate = GetProcess.StartTime.ToString("HH:mm:ss dd/MM/yyyy");
                OpenProcessToken(GetProcess.Handle, 8, out processHandle);
                var wi = new WindowsIdentity(processHandle);
                var user = wi.Name;
                User = user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
                GetFilePath = GetProcess.MainModule.FileName;
                
            }
            catch
            {
                GetFilePath = "Access denied";
                User = null;
                GetStartDate = "Access denied";
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }

        public bool CheckAvailability()
        {
            return GetStartDate != "Access denied";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
