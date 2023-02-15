using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Interfaces
{
    public interface IGameRepository
    {
        ICollection<Game> GetGames();
        Game GetGame(int id);
        Game GetGame(string name);
        bool GameExists(int id);
    }
}
