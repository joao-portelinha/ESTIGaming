using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Interfaces
{
    public interface IPlatformRepository
    {
        ICollection<Platform> GetPlatforms();
        Platform GetPlatform(int id);
        ICollection<Game> GetGamesByPlatform(int id);
        bool PlatformExists(int id);
        bool CreatePlatform(Platform platform);
        bool UpdatePlatform(Platform platform);
        bool DeletePlatform(Platform platform);
        bool Save();
    }
}
