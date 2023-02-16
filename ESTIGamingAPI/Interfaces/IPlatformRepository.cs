using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Interfaces
{
    public interface IPlatformRepository
    {
        ICollection<Platform> GetPlatforms();
        Platform GetPlatform(int id);
        ICollection<Game> GetGamesByPlatform(int id);
        bool PlatformExists(int id);
    }
}
