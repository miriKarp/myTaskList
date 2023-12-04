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


    public class taskListServices : IListTaskService
    {

        List<task> tasks { get; }

        private readonly long userId;

        private IWebHostEnvironment webHost;
        private string filePath;
        public taskListServices(IWebHostEnvironment webHost,IHttpContextAccessor httpContextAccessor)
        {
            this.userId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value);
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

        public List<task> GetAll()
        {
            return tasks.Where(p => p.taskUserId == userId).ToList();
        }

        public task Get(int id)
        {
            return tasks.FirstOrDefault(t => t.taskUserId == userId && t.id == id);
        }

        public void Add(task task)
        {
            task.id = tasks.Count() + 1;
            task.taskUserId = userId;
            tasks.Add(task);
            saveToFile();

        }
        public void Delete(int id)
        {
            var task = Get( id);
            if (task is null)
                return;
            tasks.Remove(task);
            saveToFile();

        }
        public void Update( task task)
        {
            var index = tasks.FindIndex(t => t.taskUserId == userId && t.id == task.id);
            if (index == -1)
                return;
            task.taskUserId=userId;
            tasks[index] = task;
            saveToFile();
        }

        public int Count()
        {
            return GetAll().Count();
        }
    }
}
