using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TaskManager.Navigation;
using TaskManager.Tools;

namespace TaskManager.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, IContentOwner
    {
        #region Fields
        private INavigatable _content;
        #endregion

        #region Properties
        public INavigatable Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
        #endregion

        internal MainWindowViewModel()
        {
            NavigationManager.Instance.Initialize(new TasksNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.Tasks);
        }

    }
}
