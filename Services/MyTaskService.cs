using System.Collections.Generic;
using System.Linq;
using Task.Models;
using Task.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Task.Services
{
    using Task.Models;
    public class TaskService : ITaskService
    {
        List<Task> tasks { get; }

        private IWebHostEnvironment webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
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

        public List<Task> GetAll(int UserId) => tasks.Where(t => t.UserId == UserId)?.ToList();

        public Task? Get(int Id) => tasks.FirstOrDefault(t => t.Id == Id);
        public void Post(Task t)
        {
            t.Id = tasks.Count() + 1;
            tasks.Add(t);
        }

        public void Delete(int id)
        {
            var task = Get(id);
            if (task is null)
                return;
            tasks.Remove(task);
            saveToFile();
        }

        public bool Update(Task t)
        {
            var index = tasks.FindIndex(task => t.Id == task.Id);
            if (index == -1)
                return false;
            tasks[index] = t;
            saveToFile();
            return true;
        }

        public int Count => tasks.Count();



    }

}