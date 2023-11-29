using Task.models;
using Task.interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Task.services
{


    public class taskListServices : IListTaskService
    {

        List<task> tasks { get; }

        private readonly long userId;

        private IWebHostEnvironment webHost;
        private string filePath;
        public taskListServices(IWebHostEnvironment webHost,IHttpContextAccessor httpContextAccessor)
        {
             this.userId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value);
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "data", "taskList.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                tasks = JsonSerializer.Deserialize<List<task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(tasks));
        }

        public List<task> GetAll(long userId)
        {
            return tasks.Where(p => p.taskUserId == userId).ToList();
        }

        public task Get(long userId, int id)
        {
            return tasks.FirstOrDefault(t => t.taskUserId == userId && t.id == id);
        }

        public void Add(long userId, task task)
        {
            task.id = tasks.Count() + 1;
            task.taskUserId = userId;
            tasks.Add(task);
            saveToFile();

        }
        public void Delete(long userId, int id)
        {
            var task = Get(userId, id);
            if (task is null)
                return;
            tasks.Remove(task);
            saveToFile();

        }
        public void Update(long userId, task task)
        {
            var index = tasks.FindIndex(t => t.taskUserId == userId && t.id == task.id);
            if (index == -1)
                return;

            tasks[index] = task;
            saveToFile();
        }

        public int Count(long userId)
        {
            return GetAll(userId).Count();
        }
    }
}
