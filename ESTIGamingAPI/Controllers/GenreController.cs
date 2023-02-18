using AutoMapper;
using ESTIGamingAPI.Dto;
using ESTIGamingAPI.Interfaces;
using ESTIGamingAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESTIGamingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Genre>))]
        public IActionResult GetGenres()
        {
            var genres = _mapper.Map<List<GenreDto>>(_genreRepository.GetGenres());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(genres);
        }

        [HttpGet("genreId")]
        [ProducesResponseType(200, Type = typeof(Genre))]
        [ProducesResponseType(400)]
        public IActionResult GetGenre(int genreId)
        {
            if (!_genreRepository.GenreExists(genreId))
                return NotFound();

            var genre = _mapper.Map<GenreDto>(_genreRepository.GetGenre(genreId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(genre);
        }

        [HttpGet("game/{genreId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Game>))]
        [ProducesResponseType(400)]
        public IActionResult GetGameByGenre(int genreId)
        {
            var games = _mapper.Map<List<GameDto>>(_genreRepository.GetGamesByGenre(genreId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(games);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGenre([FromBody] GenreDto genreCreate)
        {
            if (genreCreate == null)
                return BadRequest(ModelState);

            var genre = _genreRepository.GetGenres().Where(g => g.Name.Trim().ToUpper() == genreCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (genre != null)
            {
                ModelState.AddModelError("", "Esse genero já existe!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genreMap = _mapper.Map<Genre>(genreCreate);

            if (!_genreRepository.CreateGenre(genreMap))
            {
                ModelState.AddModelError("", "Erro na gravação.");
                return StatusCode(500, ModelState);
            }

            return Ok("Genero adicionado com sucesso!");
        }

        [HttpPut("{genreId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateGenre(int genreId, [FromBody]GenreDto updatedGenre)
        {
            if(updatedGenre == null)
                return BadRequest(ModelState);

            if(genreId != updatedGenre.Id) 
                return BadRequest(ModelState);

            if(!_genreRepository.GenreExists(genreId))
                return NotFound();

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var genreMap = _mapper.Map<Genre>(updatedGenre);

            if(!_genreRepository.UpdateGenre(genreMap))
            {
                ModelState.AddModelError("", "Erro ao atualizar o genero");
                return StatusCode(500, ModelState);
            }

            return Ok("Atualizou o genero " + genreId + " com sucesso");
        }

        [HttpDelete("genreId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteGenre(int genreId)
        {
            if(!_genreRepository.GenreExists(genreId))
            {
                return NotFound();
            }

            var genreToDelete = _genreRepository.GetGenre(genreId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_genreRepository.DeleteGenre(genreToDelete))
            {
                ModelState.AddModelError("", "Erro ao apagar o genero");
            }

            return Ok("Genero " + genreId + " apagado com sucesso.");
        }
    }
}
