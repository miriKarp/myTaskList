using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using mTask.models;
using mTask.interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using mTask.services;


namespace mTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Route("[action]")]
    public class taskManagerController : ControllerBase
    {
        IUserService userService;
        public taskManagerController(IUserService userService)
        {
            this.userService = userService;

        }
        [HttpPost]
        public ActionResult<String> Login([FromBody] User User)
        {
            System.Console.WriteLine(User);
            var dt = DateTime.Now;

            var user = this.userService.GetAll().FirstOrDefault(c =>
            c.UserName == User.UserName && c.Password == User.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.TaskManager ? "TaskManager" : "taskUser"),
                new Claim("Id", user.Id.ToString()),

            };
            if (user.TaskManager)
                claims.Add(new Claim("UserType", "taskUser"));

            var token = taskListTokenServices.GetToken(claims);
            System.Console.WriteLine(token);
            return new OkObjectResult(taskListTokenServices.WriteToken(token));
        }
    }
}