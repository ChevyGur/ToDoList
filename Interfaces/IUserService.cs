
namespace User.Interfaces
{
    using User.Models;
    public interface IUserService
    {
        List<User>? GetAll();
        User Get();
        void Post(User t);
        void Delete(int id);
    }
}
