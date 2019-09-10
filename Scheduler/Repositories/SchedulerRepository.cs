using Scheduler.Interfaces;
using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Repositories
{
    public class SchedulerRepository : ISchedulerRepository
    {
        IList<ScheduledTask> tasks;

        public SchedulerRepository()
        {
            tasks = new List<ScheduledTask>();
            tasks.Add(new ScheduledTask { Id = 1, Active = true, Name = "Task 1", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 2, Active = true, Name = "Task 2", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 3, Active = true, Name = "Task 2", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 4, Active = true, Name = "Task 3", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 5, Active = true, Name = "Task 4", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 6, Active = true, Name = "Task 5", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 7, Active = true, Name = "Task 6", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 8, Active = true, Name = "Task 7", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 9, Active = true, Name = "Task 8", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
        }

        public void AddScheduledTask(ScheduledTask task)
        {
            tasks.Add(task);
        }

        public ScheduledTask GetScheduledTaskById(int id)
        {
            return tasks.Where(t => t.Id == id).SingleOrDefault();
        }

        public IEnumerable<ScheduledTask> GetScheduledTasks()
        {
            return tasks;
        }

        public void RemoveSchecduledTaskById(int id)
        {
            tasks.Remove(GetScheduledTaskById(id));
        }

        public void UpdateScheduledTask(ScheduledTask task)
        {
            RemoveSchecduledTaskById(task.Id);
            AddScheduledTask(task);
        }
    }
}
