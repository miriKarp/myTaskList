using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Task.interfaces;
using Task.models;
using Microsoft.AspNetCore.Authorization;


namespace Task.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "TaskManager")]
    public class taskListControllers : ControllerBase
    {
        private long userId;
        IListTaskService taskListServices;

        public taskListControllers(IListTaskService taskListServices)
        {
            this.taskListServices = taskListServices;
            this.userId = long.Parse(User.FindFirst("UserId")?.Value ?? "");
        }

        [HttpGet]
        public ActionResult<List<task>> GetAll() =>
            taskListServices.GetAll(userId);



        [HttpGet("{id}")]
        public ActionResult<task> Get(int id)
        {
            var task = taskListServices.Get(userId, id);

            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost]
        public ActionResult Create(task newTask)
        {
            taskListServices.Add(userId, newTask);
            return CreatedAtAction(nameof(Create), new { id = newTask.id }, newTask);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, task task)
        {
            if (id != task.id)
                return BadRequest();

            var existingTask = taskListServices.Get(userId, id);
            if (existingTask is null)
                return NotFound();

            taskListServices.Update(userId, task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = taskListServices.Get(userId, id);
            if (task is null)
                return NotFound();

            taskListServices.Delete(userId, id);

            return Content(taskListServices.Count(userId).ToString());
        }




    }


}
