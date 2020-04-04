using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Views;

namespace TaskManager.Navigation
{
    internal class TasksNavigationModel : BaseNavigationModel
    {
        public TasksNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {

        }

        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Tasks:
                    AddView(ViewType.Tasks, new TasksView());
                    break;
                case ViewType.Modules:
                    AddView(ViewType.Modules, new ModulesView());
                    break;
                case ViewType.Threads:
                    AddView(ViewType.Threads, new ThreadsView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
