
using System.Collections.Generic;

namespace Task.Interfaces
{
    using Task.Models;
    public interface ITaskService
    {
        List<Task>? GetAll(int userId);
        Task Get(int id);
        void Post(Task t);
        void Delete(int id);
        bool Update(Task t);
        int Count { get; }
    }
}
