﻿namespace TaskManager.Navigation
{
    internal enum ViewType
    {
        Tasks = 0,
        Modules = 1,
        Threads = 2,
    }

    internal interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
