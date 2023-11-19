using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Task.models;
using Task.interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Task.services;


namespace Task.Controllers
{
    [ApiController]
    [Route("[action]")]
    [Route("[controller]")]
    public class taskManagerController : ControllerBase
    {
        private List<taskUser> users;
        public taskManagerController()
        {
            users = new List<taskUser>
            {
                new taskUser { UserId = 1, Username = "Avraham", Password = "A1234!", TaskManager = true},
                new taskUser { UserId = 2, Username = "Itshak", Password = "Y1234@"},
                new taskUser { UserId = 3, Username = "Yaakov", Password = "Y1234#"}
            };
        }
        [HttpPost]       
        public ActionResult<String> Login([FromBody] taskUser User)
        {
            var dt = DateTime.Now;

            var user = users.FirstOrDefault(u =>
                u.Username == User.Username
                && u.Password == User.Password
            );

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.TaskManager ? "TaskManager" : "taskUserId"),
                new Claim("userId", user.UserId.ToString()),

            };

            var token = taskListTokenServices.GetToken(claims);

            return new OkObjectResult(taskListTokenServices.WriteToken(token));
        }
    }
}