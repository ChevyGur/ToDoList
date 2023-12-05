namespace User.Services
{
    using System.Text.Json;
    using Tasks.Services;
    using User.Interfaces;
    using User.Models;
    public class UserService : IUserService
    {
        List<User> users { get; }
        private readonly int userId;

        static int nextId = 100;

        private IWebHostEnvironment webHost;
        private string filePath;
        public UserService(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        {
            this.userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value);

            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "User.json");
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

        public List<User>? GetAll() => users;

        public User? Get() => users?.FirstOrDefault(t => t.Id == userId);

        public void Post(User u)
        {
            u.Id = users[users.Count() - 1].Id + 1;
            users.Add(u);
            saveToFile();
        }

        public void Delete(int id)
        {
            var user = Get();
            if (user is null)
                return;
            users.Remove(user);
            saveToFile();
        }
    }

}
