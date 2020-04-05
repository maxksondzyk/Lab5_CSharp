using System;
using System.Diagnostics;

namespace TaskManager.Models
{
    internal class MyModule
    {
        #region Fields

        private readonly ProcessModule _module;

        #endregion

        public string Name => _module.ModuleName;

        public string Filepath
        {

            get
            {
                try
                {
                    return _module.FileName;
                }
                catch (Exception)
                {
                    return "Access denied";
                }
            }

        }

        internal MyModule(ProcessModule module)
        {
            _module = module;
        }
    }
}
