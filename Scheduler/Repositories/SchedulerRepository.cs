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
        //IList<ScheduledTask> tasks;
        private readonly SchedulerContext _context;

        public SchedulerRepository(SchedulerContext context)
        {
            _context = context;
            /*tasks = new List<ScheduledTask>();
            tasks.Add(new ScheduledTask { Id = 1, Active = true, Name = "Task 1", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 2, Active = true, Name = "Task 2", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 3, Active = true, Name = "Task 2", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 4, Active = true, Name = "Task 3", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 5, Active = true, Name = "Task 4", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 6, Active = true, Name = "Task 5", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 7, Active = true, Name = "Task 6", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 8, Active = true, Name = "Task 7", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });
            tasks.Add(new ScheduledTask { Id = 9, Active = true, Name = "Task 8", CommantToExecute = "c:\\some command.exe", StartDateTime = DateTime.Now });*/
        }

        public void AddScheduledTask(ScheduledTask task)
        {
            _context.Add(task);
            _context.SaveChanges();
        }

        public ScheduledTask GetScheduledTaskById(int id)
        {
            
            return _context.Find<ScheduledTask>(id);
        }

        public IEnumerable<ScheduledTask> GetScheduledTasks()
        {
            return _context.ScheduledTasks;
        }

        public void RemoveSchecduledTaskById(int id)
        {
            ScheduledTask task = _context.Find<ScheduledTask>(id);
            _context.Remove(task);
            _context.SaveChanges();
        }

        public void UpdateScheduledTask(ScheduledTask task)
        {
            ScheduledTask existingTask = _context.Find<ScheduledTask>(task.Id);
            existingTask.Active = task.Active;
            existingTask.Name = task.Name;
            existingTask.StartDateTime = task.StartDateTime;
            existingTask.CommantToExecute = task.CommantToExecute;
            _context.ScheduledTasks.Update(existingTask);
            _context.SaveChanges();
        }
    }
}
