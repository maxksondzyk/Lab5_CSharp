using System.Windows;

namespace TaskManager.Tools
{
    internal interface ILoaderOwner
    {
        public Visibility LoaderVisibility { get; set; }
        public bool IsControlEnabled { get; set; }
    }
}
