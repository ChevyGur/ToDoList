
namespace User.Interfaces
{
    using User.Models;
    public interface IUserService
    {
        List<User>? GetAll();
        User Get(int userId);
        void Post(User t);
        void Delete(int id);
    }
}
