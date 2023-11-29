using Tasks.Interfaces;
using System.Text.Json;

namespace Tasks.Services
{
    using Tasks.Models;
    public class TaskService : ITaskService
    {
        List<Tasks> tasks { get; }
        private readonly int UserId;
        private IWebHostEnvironment webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value); 
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

        public List<Tasks> GetAll() => tasks.Where(u => u.UserId == UserId).ToList();


        public Tasks? Get(int Id) => tasks.FirstOrDefault(t => t.Id == Id && t.UserId == UserId);

        public void Post(Tasks t)
        {
            t.UserId = (int)this.UserId;
            t.Id = tasks[tasks.Count() - 1].Id + 1;
            tasks.Add(t);
            saveToFile();
        }

        public void Delete(int id)
        {

            var task = Get(id);
            tasks.Remove(task);
            saveToFile();
        }

        public bool Update(Tasks t)
        {
            var item = tasks.Find(task => t.Id == task.Id);
            var index = tasks.FindIndex(task => task.Id == t.Id);
            t.UserId = item.UserId;
            if (index == -1)
                return false;
            tasks[index] = t;
            saveToFile();
            return true;
        }

        public int Count => tasks.Count();



    }

}