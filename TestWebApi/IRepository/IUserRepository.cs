using TestWebApi.Models;

namespace TestWebApi.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUser();

        Task<User> AddUser(User user);

        Task<User> EditUser(User user);

        Task<string> DeleteUser(User user);

    }
}
