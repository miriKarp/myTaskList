using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mTask.interfaces;
using mTask.models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace mTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "taskUser")]
    public class taskListControllers : ControllerBase
    {
        // private long userId;
        IListTaskService taskListServices;

        public taskListControllers(IListTaskService taskListServices)
        {
            this.taskListServices = taskListServices;
            // this.userId = long.Parse(User.FindFirst("UserId")?.Value ?? "");
        }

        [HttpGet]
        public ActionResult<List<task>> GetAll() =>
            taskListServices.GetAll();

        [HttpGet("{id}")]
        public ActionResult<task> Get(int id)
        {
            var task = taskListServices.Get(id);

            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost]
        public ActionResult Create(task newTask)
        {
            taskListServices.Add(newTask);
            return CreatedAtAction(nameof(Create), new { id = newTask.id }, newTask);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, task task)
        {
            if (id != task.id)
                return BadRequest();

            var existingTask = taskListServices.Get(id);
            if (existingTask is null)
                return NotFound();

            taskListServices.Update(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = taskListServices.Get(id);
            System.Console.WriteLine("task! ", task);
            if (task is null)
                return NotFound();

            taskListServices.Delete(id);

            return Content(taskListServices.Count().ToString());
        }




    }


}
