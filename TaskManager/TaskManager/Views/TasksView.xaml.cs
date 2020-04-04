﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Navigation;
using TaskManager.Tools;
using TaskManager.ViewModels;

namespace TaskManager.Views
{
    /// <summary>
    /// Interaction logic for TasksView.xaml
    /// </summary>
    public partial class TasksView : UserControl, INavigatable
    {
        public TasksView()
        {
            InitializeComponent();
            DataContext = new TasksViewModel();
        }
    }
}