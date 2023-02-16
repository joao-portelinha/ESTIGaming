using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        bool UserExists(int id);
    }
}
