using ESTIGamingAPI.Data;
using ESTIGamingAPI.Interfaces;
using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public bool GameExists(int gameId)
        {
            return _context.Games.Any(g => g.Id == gameId);
        }

        public Game GetGame(int id)
        {
            return _context.Games.Where(g => g.Id == id).FirstOrDefault();
        }

        public Game GetGame(string name)
        {
            return _context.Games.Where(g => g.Name == name).FirstOrDefault();
        }

        public ICollection<Game> GetGames()
        {
            return _context.Games.OrderBy(g => g.Id).ToList();
        }

        public bool CreateGame(Game game)
        {
            _context.Add(game);
            return Save();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
