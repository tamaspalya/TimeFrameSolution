using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeFrameAPI.DTOs;
using TimeFrameLib.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeFrameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        IUserRepository _userRepository;

        public LoginsController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST api/<LoginsController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] LoginDto login)
        {
            return await _userRepository.LoginAsync(login.Email, login.Password);
        }
    }
}
