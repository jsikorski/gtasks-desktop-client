using System;
using System.Collections.Generic;
using GTasksDesktopClient.Core.Lists;
using Google.Apis.Tasks.v1.Data;

namespace GTasksDesktopClient.Core.Layout
{
    public class LayoutViewModel
    {
        public TasksListsViewModel TasksListsViewModel { get; set; }

        public LayoutViewModel(TasksListsViewModel tasksListsViewModel)
        {
            TasksListsViewModel = tasksListsViewModel;
        }
    }
}