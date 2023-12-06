using mTask.models;
using mTask.interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace mTask.services
{

    public class userService : IUserService
    {

        List<User> users { get; }

        private IWebHostEnvironment webHost;
        private string filePath;
        IListTaskService taskListServices;

        public userService(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor, IListTaskService taskListServices)
        {
            this.taskListServices = taskListServices;
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "data", "users.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }

        public List<User> GetAll() => users;

        public User Get(long userId) => users?.FirstOrDefault(u => u.Id == userId);

        public void Post(User u)
        {
            u.Id = users[users.Count() - 1].Id + 1;
            u.TaskManager = false;
            users.Add(u);
            saveToFile();
        }

        public void Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return;
            users.Remove(user);
            List<task> tasks = taskListServices.GetAll(user.Id);
            foreach (var t in tasks)
            {
                taskListServices.Delete(user.Id, t.id);
            }
            saveToFile();
        }
    }
}