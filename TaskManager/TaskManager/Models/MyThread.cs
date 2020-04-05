using System;
using System.Diagnostics;

namespace TaskManager.Models
{
    class MyThread
    {
        #region Fields

        private readonly ProcessThread _thread;

        #endregion

        public int Id => _thread.Id;

        public ThreadState State => _thread.ThreadState;

        public string StartingTime
        {
            get
            {
                try
                {
                    return _thread.StartTime.ToString("HH:mm:ss dd/MM/yyyy");
                }
                catch (Exception)
                {
                    return "Access denied";
                }
            }
        }
        internal MyThread(ProcessThread thread)
        {
            _thread = thread;
        }
    }
}
