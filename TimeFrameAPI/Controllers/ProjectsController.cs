using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeFrameAPI.DTOs;
using TimeFrameAPI.DTOs.Converters;
using TimeFrameLib.Models;
using TimeFrameLib.Repositories;

namespace TimeFrameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        #region Repository and constructor
        IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] ProjectDto newProjectDto)
        {
            return Ok(await _projectRepository.CreateProjectAsync(newProjectDto.FromDto()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAll()
        {
            var projects = await _projectRepository.GetAllAsync();

            return Ok(projects.ToList().ToDtos());

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetById(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);

            return Ok(project.ToDto());
        }

        [HttpGet("Team/{teamId}")]
        public async Task<ActionResult<Project>> GetByTeamId(int teamId)
        {
            var projects = await _projectRepository.GetProjectsByTeamIdAsync(teamId);

            return Ok(projects.ToDtos());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteById(int id)
        {
            return Ok(await _projectRepository.DeleteProjectAsync(id));
        }
        #endregion
    }
}
