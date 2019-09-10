using Scheduler.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Scheduler.Models
{
    public class ScheduledTask
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [FutureDateAttribute(ErrorMessage = "Date should be in the future.")]
        public DateTime StartDateTime { get; set; }
        [Required]
        public string CommantToExecute { get; set; }
        public bool Active { get; set; }
    }
}
