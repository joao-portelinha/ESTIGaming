using AutoMapper;
using ESTIGamingAPI.Data;
using ESTIGamingAPI.Interfaces;
using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Repository
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly DataContext _context;

        public PlatformRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Game> GetGamesByPlatform(int id)
        {
            return _context.Games.Where(g => g.Platform.Id == id).ToList();
        }

        public Platform GetPlatform(int id)
        {
            return _context.Platforms.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Platform> GetPlatforms()
        {
            return _context.Platforms.OrderBy(p => p.Id).ToList();
        }

        public bool PlatformExists(int id)
        {
            return _context.Platforms.Any(p => p.Id == id);
        }

        public bool CreatePlatform(Platform platform)
        {
            _context.Add(platform);
            return Save();
        }

        public bool UpdatePlatform(Platform platform)
        {
            _context.Update(platform);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeletePlatform(Platform platform)
        {
            _context.Remove(platform);
            return Save();
        }
    }
}
