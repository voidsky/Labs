using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scheduler.Interfaces;
using Scheduler.Models;

namespace Scheduler.Controllers
{
    public class ScheduledTasksController : Controller
    {
        private readonly ISchedulerRepository _repo;

        public ScheduledTasksController(ISchedulerRepository repo)
        {
            _repo = repo;
        }

        // GET: ScheduledTasks
        public IActionResult Index()
        {
            return View(_repo.GetScheduledTasks());
        }

        // GET: ScheduledTasks/Details/5
        public IActionResult Details(int id)
        {
            var scheduledTask = _repo.GetScheduledTaskById(id);
            if (scheduledTask == null)
            {
                return NotFound();
            }

            return View(scheduledTask);
        }

        // GET: ScheduledTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScheduledTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDateTime,CommantToExecute,Active")] ScheduledTask scheduledTask)
        {
            if (ModelState.IsValid)
            {
                _repo.AddScheduledTask(scheduledTask);
                return RedirectToAction(nameof(Index));
            }
            return View(scheduledTask);
        }

        // GET: ScheduledTasks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var scheduledTask = _repo.GetScheduledTaskById(id);
            if (scheduledTask == null)
            {
                return NotFound();
            }
            return View(scheduledTask);
        }

        // POST: ScheduledTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDateTime,CommantToExecute,Active")] ScheduledTask scheduledTask)
        {
            if (id != scheduledTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repo.UpdateScheduledTask(scheduledTask);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(scheduledTask);
        }

        // GET: ScheduledTasks/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledTask = _repo.GetScheduledTaskById(id);
            if (scheduledTask == null)
            {
                return NotFound();
            }

            return View(scheduledTask);
        }

        // POST: ScheduledTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _repo.RemoveSchecduledTaskById(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
