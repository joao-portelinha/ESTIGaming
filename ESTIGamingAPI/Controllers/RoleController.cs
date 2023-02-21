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
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRoles()
        {
            var roles = _mapper.Map<List<RoleDto>>(_roleRepository.GetRoles());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles);
        }

        [HttpGet("roleId")]
        [ProducesResponseType(200, Type = typeof(Role))]
        [ProducesResponseType(400)]
        public IActionResult GetRole(int roleId)
        {
            if (!_roleRepository.RoleExists(roleId))
                return NotFound();

            var role = _mapper.Map<RoleDto>(_roleRepository.GetRole(roleId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(role);
        }

        [HttpGet("user/{roleId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserByRole(int roleId)
        {
            var users = _mapper.Map<List<UserDto>>(_roleRepository.GetUsersByRole(roleId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }
    }
}
