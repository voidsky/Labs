using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Scheduler.Models
{
    public class SchedulerContext : DbContext
    {
        public SchedulerContext (DbContextOptions<SchedulerContext> options)
            : base(options)
        {
        }

        public DbSet<Scheduler.Models.ScheduledTask> ScheduledTasks { get; set; }
    }
}
