using ESTIGamingAPI.Data;
using ESTIGamingAPI.Interfaces;
using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context)
        {
            _context = context;
        }

        public bool GenreExists(int id)
        {
            return _context.Genres.Any(g => g.Id == id);
        }

        public ICollection<Game> GetGamesByGenre(int id)
        {
            return _context.Games.Where(g => g.Genre.Id == id).ToList();
        }

        public Genre GetGenre(int id)
        {
            return _context.Genres.Where(g => g.Id == id).FirstOrDefault();
        }

        public ICollection<Genre> GetGenres()
        {
            return _context.Genres.OrderBy(g => g.Id).ToList();
        }

        public bool CreateGenre(Genre genre)
        {
            _context.Add(genre);
            return Save();
        }

        public bool UpdateGenre(Genre genre)
        {
            _context.Update(genre);
            return Save();
        }

        public bool DeleteGenre(Genre genre)
        {
            _context.Remove(genre);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
