using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using mTask.models;
using mTask.interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using mTask.services;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace mTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
        // [Route("[action]")]
    public class userController : ControllerBase
    {

        private readonly long userId;

        IUserService userService;
        public userController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.userId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value);

        }
        [HttpGet]
        [Authorize(Policy = "TaskManager")]
        public ActionResult<List<User>> GetAll() => userService.GetAll();

        // [HttpGet]
        // [Authorize(Policy = "taskUser")]
        // public ActionResult<List<User>> Get() => userService.Get();

        [HttpGet("{id}")]
        [Authorize(Policy = "taskUser")]
        public ActionResult<User> GetMyUser()
        {
            var user = userService.Get(userId);
            if (user == null)
                return NotFound();
            return user;
        }

        // [HttpPost("{user}")]
        [HttpPost]
        [Authorize(Policy = "TaskManager")]
        public ActionResult Post([FromBody] User user)
        {
            userService.Post(user);
            return CreatedAtAction(nameof(Post), new { Id = user.Id }, user);
        }

        [HttpDelete]
        [Authorize(Policy = "TaskManager")]
        public ActionResult Delete(int id)
        {
            var user = userService.Get(id);
            if (user == null)
                return NotFound();
            userService.Delete(id);
            return NoContent();
        }

    }

}