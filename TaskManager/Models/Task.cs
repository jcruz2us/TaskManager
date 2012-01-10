using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Task : Entity
    {
        [Required]
        [Display(Name = "Task Description")]
        public String Description { get; set; }

        [Display(Name = "Check if the task completed")]
        public bool IsCompleted { get; set; }
    }
}