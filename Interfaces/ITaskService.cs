
using System.Collections.Generic;

namespace Tasks.Interfaces
{
    using Tasks.Models;
    public interface ITaskService
    {
        List<Tasks>? GetAll();
        Tasks Get(int id);
        void Post(Tasks t);
        void Delete(int id);
        bool Update(Tasks t);
        int Count { get; }
    }
}
