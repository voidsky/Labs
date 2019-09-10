using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Interfaces
{
    public interface ISchedulerRepository
    {
        IEnumerable<ScheduledTask> GetScheduledTasks();
        ScheduledTask GetScheduledTaskById(int id);
        void AddScheduledTask(ScheduledTask task);
        void UpdateScheduledTask(ScheduledTask task);
        void RemoveSchecduledTaskById(int id);
    }
}
