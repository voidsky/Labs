using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Scheduler.Models;
using Scheduler.Repositories;
using System.Linq;

namespace Test
{
    public class SchedulerRepositoryTests
    {
        [Fact]
        public void Add_CreatesNewTask()
        {
            var options = new DbContextOptionsBuilder<SchedulerContext>()
                .UseInMemoryDatabase(databaseName: "Add_CreatesNewTask")
                .Options;

            using (var context = new SchedulerContext(options))
            {
                SchedulerRepository repo = new SchedulerRepository(context);
                repo.AddScheduledTask(new ScheduledTask
                {
                    Id = 1,
                    Active = true,
                    Name = "some task",
                    CommantToExecute = "some command",
                    StartDateTime = DateTime.Now.AddYears(1)
                });
            }

            using (var context = new SchedulerContext(options))
            {
                SchedulerRepository repo = new SchedulerRepository(context);
                var tasks = repo.GetScheduledTasks();
                Assert.Single(tasks);
            }
        }

        [Fact]
        public void Update_UpdatesTask()
        {
            var options = new DbContextOptionsBuilder<SchedulerContext>()
                .UseInMemoryDatabase(databaseName: "Update_UpdatesTask")
                .Options;

            using (var context = new SchedulerContext(options))
            {
                SchedulerRepository repo = new SchedulerRepository(context);
                var task = new ScheduledTask
                {
                    Id = 1,
                    Active = true,
                    Name = "some task",
                    CommantToExecute = "some command",
                    StartDateTime = DateTime.Now.AddYears(1)
                };

                repo.AddScheduledTask(task);
                task.Name = "updated";
                repo.UpdateScheduledTask(task);
            }

            using (var context = new SchedulerContext(options))
            {
                SchedulerRepository repo = new SchedulerRepository(context);
                var task = repo.GetScheduledTaskById(1);
                Assert.Equal("updated", task.Name);
            }
        }

        [Fact]
        public void Delete_RemovesTask()
        {
            var options = new DbContextOptionsBuilder<SchedulerContext>()
                .UseInMemoryDatabase(databaseName: "Delete_RemovesTask")
                .Options;


            using (var context = new SchedulerContext(options))
            {
                SchedulerRepository repo = new SchedulerRepository(context);
                var task = new ScheduledTask
                {
                    Id = 1,
                    Active = true,
                    Name = "some task",
                    CommantToExecute = "some command",
                    StartDateTime = DateTime.Now.AddYears(1)
                };

                repo.AddScheduledTask(task);
                repo.RemoveSchecduledTaskById(1);
            }

            using (var context = new SchedulerContext(options))
            {
                SchedulerRepository repo = new SchedulerRepository(context);
                var task = repo.GetScheduledTasks();
                Assert.Empty(task);
            }
        }

        [Fact]
        public void Get_GetsAllTasks()
        {
            var options = new DbContextOptionsBuilder<SchedulerContext>()
                .UseInMemoryDatabase(databaseName: "Get_GetsAllTasks")
                .Options;


            using (var context = new SchedulerContext(options))
            {
                SchedulerRepository repo = new SchedulerRepository(context);
                for (int i = 1; i <= 100; i++)
                {
                    var task = new ScheduledTask
                    {
                        Id = i,
                        Active = true,
                        Name = "some task",
                        CommantToExecute = "some command",
                        StartDateTime = DateTime.Now.AddYears(1)
                    };
                    repo.AddScheduledTask(task);
                }
            }

            using (var context = new SchedulerContext(options))
            {
                SchedulerRepository repo = new SchedulerRepository(context);
                var task = repo.GetScheduledTasks();
                Assert.Equal(100, task.Count<ScheduledTask>());
            }
        }

    }
}
