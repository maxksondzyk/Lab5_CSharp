using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TaskManager.Models
{
    class MyThread
    {
        #region Fields

        private readonly ProcessThread _thread;

        #endregion

        public int Id
        {
            get { return _thread.Id; }
        }

        public ThreadState State
        {

            get
            {
                return _thread.ThreadState;

            }

        }

        public string StartingTime
        {
            get
            {
                try
                {
                    return _thread.StartTime.ToString("HH:mm:ss dd/MM/yyyy"); ;
                }
                catch (Exception e)
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
