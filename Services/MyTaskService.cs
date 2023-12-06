using Tasks.Interfaces;
using System.Text.Json;

namespace Tasks.Services
{
    using Microsoft.Extensions.Configuration.UserSecrets;
    using Tasks.Models;
    public class TaskService : ITaskService
    {
        List<Tasks> tasks { get; }
        private IWebHostEnvironment webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                tasks = JsonSerializer.Deserialize<List<Tasks>>(jsonFile.ReadToEnd(),
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

        public List<Tasks> GetAll(int userId) => tasks.Where(u => u.UserId == userId).ToList();


        public Tasks? Get(int userId, int Id) => tasks.FirstOrDefault(t => t.Id == Id && t.UserId == userId);

        public void Post(int userId, Tasks t)
        {
            t.UserId = userId;
            t.Id = tasks[^1].Id + 1;
            tasks.Add(t);
            saveToFile();
        }

        public void Delete(int userId, int id)
        {

            var task = Get(userId, id);
            tasks.Remove(task);
            saveToFile();
        }

        public void  Update(int userId, Tasks t)
        {
            try
            {
                var index = tasks.FindIndex(task => task.Id == t.Id);
                t.UserId = userId;
                tasks[index] = t;
                saveToFile();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int Count => tasks.Count();



    }

}