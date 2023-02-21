using AutoMapper;
using ESTIGamingAPI.Dto;
using ESTIGamingAPI.Filter;
using ESTIGamingAPI.Interfaces;
using ESTIGamingAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESTIGamingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenFilter]
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;

        public GameController(IGameRepository gameRepository, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Game>))]
        public IActionResult GetGames()
        {
            var games = _mapper.Map<List<GameDto>>(_gameRepository.GetGames());  

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(games);
        }

        [HttpGet("{gameId}")]
        [ProducesResponseType(200, Type = typeof(Game))]
        [ProducesResponseType(400)]
        public IActionResult GetGame(int gameId) 
        { 
            if (!_gameRepository.GameExists(gameId))
                return NotFound();

            var game = _mapper.Map<GameDto>(_gameRepository.GetGame(gameId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(game);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGame([FromBody] GameDto gameCreate)
        {
            if (gameCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gameMap = _mapper.Map<Game>(gameCreate);

            if (!_gameRepository.CreateGame(gameMap))
            {
                ModelState.AddModelError("", "Erro na gravação.");
                return StatusCode(500, ModelState);
            }

            return Ok("Jogo adicionado com sucesso!");
        }

        [HttpPut("{gameId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateGame(int gameId, [FromBody] GameDto updatedGame) 
        {
            if(updatedGame == null)
                return BadRequest(ModelState);

            if(gameId != updatedGame.Id) 
                return BadRequest(ModelState);

            if(!_gameRepository.GameExists(gameId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var gameMap = _mapper.Map<Game>(updatedGame);

            if (!_gameRepository.UpdateGame(gameMap))
            {
                ModelState.AddModelError("", "Erro ao atualizar o jogo");
                return StatusCode(500, ModelState);
            }

            return Ok("Atualizou o jogo "  + gameId + " com sucesso");
        }

        [HttpDelete("gameId")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteGame(int gameId)
        {
            if (!_gameRepository.GameExists(gameId))
            {
                return NotFound();
            }

            var gameToDelete = _gameRepository.GetGame(gameId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_gameRepository.DeleteGame(gameToDelete))
            {
                ModelState.AddModelError("", "Erro ao apagar o jogo");
            }

            return Ok("Jogo " + gameId + " apagado com sucesso.");
        }
    }
}
