using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Navigation
{
    internal interface IContentOwner
    {
        INavigatable Content { get; set; }
    }
}
