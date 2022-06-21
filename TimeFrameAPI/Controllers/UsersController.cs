using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeFrameAPI.DTOs;
using TimeFrameLib.Repositories;
using TimeFrameAPI.DTOs.Converters;
using TimeFrameLib.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeFrameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        // GET: api/<UsersController>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var users = await _userRepository.GetAllAsync();

            return Ok(users.ToList().ToDtos());
            
        }
        

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) { return NotFound(); }
            else { return Ok(user.ToDto()); }
        }

        // GET api/<UsersController>/email
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery]string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) { return NotFound(); }
            else { return Ok(user.ToDto()); }
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] RegisterDto registerDto)
        {
            return Ok(await _userRepository.CreateAsync(new User { Email = registerDto.Email, FirstName = registerDto.FirstName, LastName = registerDto.LastName } , registerDto.Password));
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UserDto userDtoToUpdate)
        {
            if (!await _userRepository.UpdateAsync(id, userDtoToUpdate.FromDto())) { return NotFound(); }
            { return Ok(); }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _userRepository.DeleteAsync(id)) { return NotFound(); }
            else { return Ok(); }
        }
    }
}
