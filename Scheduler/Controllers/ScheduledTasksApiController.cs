using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Scheduler.Interfaces;
using Scheduler.Models;

namespace Scheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledTasksApiController : ControllerBase
    {
        private readonly ISchedulerRepository _repo;

        public ScheduledTasksApiController(ISchedulerRepository repo)
        {
            _repo = repo;
        }

        // GET: api/ScheduledTasksApi
        [HttpGet]
        public IEnumerable<ScheduledTask> Get()
        {
            return _repo.GetScheduledTasks();
        }

        // GET: api/ScheduledTasksApi/5
        [HttpGet("{id}", Name = "Get")]
        public ScheduledTask Get(int id)
        {
            return _repo.GetScheduledTaskById(id);
        }

        // POST: api/ScheduledTasksApi
        [HttpPost]
        public void Post([FromBody] ScheduledTask value)
        {
            _repo.AddScheduledTask(value);
        }

        // PUT: api/ScheduledTasksApi/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ScheduledTask value)
        {
            value.Id = id;
            _repo.UpdateScheduledTask(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.RemoveSchecduledTaskById(id);
        }

    }
}
