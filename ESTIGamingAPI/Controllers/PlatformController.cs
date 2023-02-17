using AutoMapper;
using ESTIGamingAPI.Dto;
using ESTIGamingAPI.Interfaces;
using ESTIGamingAPI.Models;
using ESTIGamingAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ESTIGamingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : Controller
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public PlatformController(IPlatformRepository platformRepository, IMapper mapper)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Platform>))]
        public IActionResult GetPlatforms()
        {
            var platforms = _mapper.Map<List<PlatformDto>>(_platformRepository.GetPlatforms());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(platforms);
        }

        [HttpGet("platformId")]
        [ProducesResponseType(200, Type = typeof(Platform))]
        [ProducesResponseType(400)]
        public IActionResult GetPlatform(int platformId)
        {
            if (!_platformRepository.PlatformExists(platformId))
                return NotFound();

            var platform = _mapper.Map<PlatformDto>(_platformRepository.GetPlatform(platformId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(platform);
        }

        [HttpGet("game/{platformId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Game>))]
        [ProducesResponseType(400)]
        public IActionResult GetGameByPlatform(int platformId)
        {
            var games = _mapper.Map<List<GameDto>>(_platformRepository.GetGamesByPlatform(platformId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(games);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePlatform([FromBody] PlatformDto platformCreate)
        {
            if (platformCreate == null)
                return BadRequest(ModelState);

            var platform = _platformRepository.GetPlatforms().Where(p => p.Name.Trim().ToUpper() == platformCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (platform != null)
            {
                ModelState.AddModelError("", "Essa plataforma já existe!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var platformMap = _mapper.Map<Platform>(platformCreate);

            if (!_platformRepository.CreatePlatform(platformMap))
            {
                ModelState.AddModelError("", "Erro na gravação.");
                return StatusCode(500, ModelState);
            }

            return Ok("Plataforma adicionada com sucesso!");
        }
    }
}
