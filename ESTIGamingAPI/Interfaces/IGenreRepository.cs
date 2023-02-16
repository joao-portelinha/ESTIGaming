﻿using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Interfaces
{
    public interface IGenreRepository
    {
        ICollection<Genre> GetGenres();
        Genre GetGenre(int id);
        ICollection<Game> GetGamesByGenre(int id);
        bool GenreExists(int id);
    }
}
