using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Interfaces
{
    public interface IGameRepository
    {
        ICollection<Game> GetGames();
    }
}
