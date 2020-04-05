using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace TaskManager.Models
{
    internal class MyProcess
    {
        #region Fields

        private readonly Process _process;
        //private readonly string _name;
        //private readonly int _id;
        //private readonly bool _isActive;
        private double _cpu;
        //private readonly float _ram;
        //private readonly int _threads;
        //private readonly string _filePath;
        //private readonly DateTime _startDate;
        private PerformanceCounter perfCounter;
        #endregion

        #region Properties

        public Process GetProcess => _process;
        public string GetName => _process.ProcessName;
        public int GetId => _process.Id;
        public bool IsActive => _process.Responding;
        public double GetCpu
        {
            get =>
                Math.Round((double)perfCounter.NextValue() / Environment.ProcessorCount, 1,
                    MidpointRounding.ToEven);
            set
            {
                _cpu = value;
            }
        }

        public float GetRam => _process.WorkingSet64/1024/1024;
        public ProcessThreadCollection GetThreads => _process.Threads;
        public int GetThreadsNum => _process.Threads.Count;

        public ProcessModuleCollection Modules => _process.Modules;

        public ProcessThreadCollection ThreadsCollection => _process.Threads;

        public string GetFilePath
        {
            get
            {
                try
                {
                    return _process.MainModule.FileName;
                }
                catch (Exception)
                {
                    return "Access denied";
                }
            }
        }

        public string GetStartDate
        {
            get
            {
                try
                {
                    return _process.StartTime.ToString("HH:mm:ss dd/MM/yyyy");
                }
                catch (Exception e)
                {
                    return "Access denied";
                }
            }
        }
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        public string User
        {
            get
            {
                IntPtr processHandle = IntPtr.Zero;
                try
                {
                    OpenProcessToken(_process.Handle, 8, out processHandle);
                    WindowsIdentity wi = new WindowsIdentity(processHandle);
                    string user = wi.Name;
                    return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    if (processHandle != IntPtr.Zero)
                    {
                        CloseHandle(processHandle);
                    }
                }
            }
        }
        #endregion
        internal void Update()
        {
            try
            {
               GetCpu =
                    Math.Round((double)perfCounter.NextValue() / Environment.ProcessorCount, 1,
                        MidpointRounding.ToEven);
            }
            catch (Exception)
            {
                GetCpu = 0;
            }
        }
        internal MyProcess(Process process)
        {
            _process = process;
            perfCounter = new PerformanceCounter("Process", "% Processor Time", "chrome");
            perfCounter.NextValue();
        }
        public bool checkAvailability()
        {
            if (GetStartDate == "Access denied")
            {
                return false;
            }
            return true;
        }
    }
}
