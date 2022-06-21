using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeFrameAPI.DTOs;
using TimeFrameAPI.DTOs.Converters;
using TimeFrameLib.Models;
using TimeFrameLib.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeFrameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        ITeamRepository _teamRepository;
        IUserRepository _userRepository;


        public TeamsController(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAllAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            return Ok(teams.ToList().ToDtos());
        }

        [HttpGet("Users/{userId}")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetByUserIdAsync(int userId)
        {
            var teams = await _teamRepository.GetByUserIdAsync(userId);
            return Ok(teams.ToList().ToDtos());
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetAsync(int id)
        {
            var team = await _teamRepository.GetByIDAsync(id);
            return Ok(team.ToDto());
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<int>> PostAsync([FromBody]  TeamDto newTeamDto)
        {       
            return Ok(await _teamRepository.CreateAsync(new Team { TeamName = newTeamDto.TeamName, TeamMembers = newTeamDto.FromDto().TeamMembers }));
        }

        // PUT api/<ValuesController>
        [HttpPut]
        public async Task<ActionResult<bool>> PutAsync([FromBody] TeamDto value)
        {
            return Ok(await _teamRepository.UpdateTeamAsync(value.FromDto()));
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            return Ok(await _teamRepository.DeleteAsync(id));
        }

        [HttpPut("{teamId}/{userId}")]
        public async Task<ActionResult<bool>> RemoveUserAsync(int userId, int teamId)
        {
            return Ok(await _teamRepository.RemoveUserAsync(userId, teamId));
        }
        [HttpGet("{teamId}/users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByTeam(int teamId)
        {
            Team team = new Team() { Id = teamId };
            return Ok(await _teamRepository.GetByTeamAsync(team));
        }
    }
}
