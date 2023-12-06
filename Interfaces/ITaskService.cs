
using System.Collections.Generic;

namespace Tasks.Interfaces
{
    using Tasks.Models;
    public interface ITaskService
    {
        List<Tasks>? GetAll(int userId);
        Tasks Get(int userId,int id);
        void Post(int userId,Tasks t);
        void Delete(int userId,int id);
        void Update(int userId,Tasks t);
        int Count { get; }
    }
}
