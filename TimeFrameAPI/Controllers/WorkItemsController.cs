using Microsoft.AspNetCore.Mvc;
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

    //where {id} is used in this file as a variable name, it always refers to the WorkItem id

    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        IWorkItemRepository _workItemRepository;
        public WorkItemsController(IWorkItemRepository workItemRepository)
        {
            _workItemRepository = workItemRepository;
        }

        #region Http Methods
        // GET api/<UsersController>/id
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkItemDto>> GetByIdAsync(int id)
        {
            var workItem = await _workItemRepository.GetByIdAsync(id);
            if (workItem != null) { return Ok(workItem.ToDto()); }
            return NotFound();
        }

        // GET api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkItemDto>>> GetAllAsync()
        {
            var users = await _workItemRepository.GetAllAsync();
            return Ok(users.ToDtos());
        }


        //POST api/WorkItemsController
        [HttpPost]
        public async Task<ActionResult<int>> PostAsync([FromBody] WorkItemDto workItemDto)
        {
            var workItem = workItemDto.FromDto();
            return Ok(await _workItemRepository.CreateWorkItemAsync(workItem));
        }

        // PUT api/<WorkItemsController>/id
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> PutAsync(int id, [FromBody] WorkItemDto workItemDtoToUpdate)
        {
            bool result = await _workItemRepository.UpdateAsync(id, workItemDtoToUpdate.FromDto());
            if (result) return Ok(result);
            return NotFound();
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<bool>> PutConcurrentlyAsync(int id, [FromBody] WorkItemDto workItemDtoToUpdate)
        {
            bool result = await _workItemRepository.UpdateConcurrentlyInTransactionAsync(id, workItemDtoToUpdate.FromDto());
            if (result) return Ok(result);
            return NotFound();
        }

        //DELETE api/<WorkItemsController>/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            bool result = await _workItemRepository.DeleteAsync(id);
            if (result) return Ok(result);
            return NotFound();
        }

        //GET api/WorkItemsController/PeopleAssigned/userId
        [HttpGet("PeopleAssigned/{userId}/Tasks")]
        public async Task<ActionResult<IEnumerable<WorkItemDto>>> GetByUserIdAsync(int userId)
        {

            var workItems = await _workItemRepository.GetByUserIdAsync(userId);
            return Ok(workItems.ToList().ToDtos());
        }

        //GET api/WorkItemsController/Project
        [HttpGet("Project/{projectId}")]
        public async Task<ActionResult<IEnumerable<WorkItemDto>>> GetByProjectAsync(int projectId)
        {
            var workItems = await _workItemRepository.GetByProjectAsync(projectId); 
            return Ok(workItems.ToList().ToDtos());
        }

        //GET api/WorkItemsController/PeopleAssigned
        [HttpGet("PeopleAssigned/{taskId}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByWorkItemAsync(int taskId)
        {
            var users = await _workItemRepository.GetUsersByWorkItemAsync(taskId);
            return Ok(users.ToList().ToDtos());
        }

        //PUT api/WorkItemsController/id/Assign
        [HttpPut("{id}/Assign")]
        public async Task<ActionResult<bool>> AssignUserAsync(int id, [FromBody] UserDto user)
        {
            bool result = await _workItemRepository.AssignUserAsync(id, user.FromDto());
            if(result) return Ok(result);
            return NotFound();
        }

        //PUT api/WorkItemsController/id/Unassign
        [HttpPut("{id}/Unassign")]
        public async Task<ActionResult<bool>> UnassignUserAsync(int id, [FromBody] UserDto user)
        {
            bool result = await _workItemRepository.UnassignUserAsync(id, user.FromDto());
            if (result) return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}/search={searchstring}")]
        public async Task<ActionResult<IEnumerable<WorkItemDto>>> SearchUsersWorkItems(int id, string searchstring)
        {
            IEnumerable<WorkItemDto> foundWorkItems = (await _workItemRepository.SearchUsersWorkItems(id, searchstring)).ToDtos();
            return Ok(foundWorkItems);
            
        }
        #endregion


    }
}
