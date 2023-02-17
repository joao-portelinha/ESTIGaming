using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetRoles();
        Role GetRole(int id);
        ICollection<User> GetUsersByRole(int id);
        bool RoleExists(int id);
    }
}
