using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class TaskIndex
    {
        public List<Task> PendingTasks { get; set; }
        public List<Task> CompletedTasks { get; set; }

        public TaskIndex(IEnumerable<Task> tasks)
        {
            PendingTasks = tasks.Where(task => task.IsCompleted == false).ToList();
            CompletedTasks = tasks.Where(task => task.IsCompleted).ToList();
        }
    }
}