using Microsoft.EntityFrameworkCore;
using TestWebApi.IRepository;
using TestWebApi.Models;

namespace TestWebApi.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly UserDbContext context;

        public UserRepository(UserDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> AddUser(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> EditUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<string> DeleteUser(User user)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return "User Deleted Successfully.";
        }
    }
}
