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
        // static int nextId=100;
        // private readonly long userId;
        private IWebHostEnvironment webHost;
        private string filePath;

        public userService(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        {
            // System.Console.WriteLine("ggggggggggggggggggggg");
            // var x=httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value;
            // System.Console.WriteLine(x);
            // this.userId = long.Parse("222");
            // System.Console.WriteLine("vhgh",this.userId);
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

        public User Get(long userId)=>users?.FirstOrDefault(u=>u.Id==userId);

        public void Post(User u)
        {
            u.Id = users[users.Count() - 1].Id + 1;
            u.TaskManager = false;
            users.Add(u);
            saveToFile();
        }

        public void Delete(int id){
            var user=Get(id);
            if(user is null)
                return;
            users.Remove(user);
            saveToFile();
        }
    }
}