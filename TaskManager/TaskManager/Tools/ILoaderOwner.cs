using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TaskManager.Tools
{
    interface ILoaderOwner
    {
        Visibility LoaderVisibility { get; set; }
        bool IsControlEnabled { get; set; }
    }
}
