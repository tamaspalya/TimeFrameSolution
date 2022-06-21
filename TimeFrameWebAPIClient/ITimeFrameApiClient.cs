using System.Collections.Generic;
using System.Threading.Tasks;
using TimeFrameWebAPIClient.DTOs;

namespace TimeFrameWebAPIClient
{
    public interface ITimeFrameApiClient
    {
        #region User functionality 
        Task<int> CreateUserAsync(RegisterDto entity);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<int> LoginAsync(LoginDto user);
        Task<bool> UpdateUserAsync(UserDto entity);
        Task<bool> UpdateUserPasswordAsync(int userid, string oldpassword, string newpassword);
        #endregion

        #region Project functionality
        Task<int> CreateProjectAsync(ProjectDto entity);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectDto>> GetProjectsByTeamIdAsync(int teamId);
        Task<bool> DeleteProjectAsync(int id);
        //Task<int> CreateProjectAsync(ProjectDto entity);  //TODO: Add ProjectDto class

        #endregion

        #region Team functionality
        Task<int> CreateTeamAsync(TeamDto TeamDto);
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task<TeamDto> GetTeamByIdAsync(int Id);
        Task<bool> DeleteTeamAsync(int Id);
        Task<bool> AddUserToTeamAsync(UserDto user, TeamDto TeamDto);
        Task<bool> RemoveUserFromTeamAsync(UserDto user, TeamDto TeamDto);
        Task<IEnumerable<TeamDto>> GetTeamsFromUserIdAsync(int userId);
        Task<IEnumerable<UserDto>> GetUsersByTeamAsync(TeamDto TeamDto);
        Task<bool> ChangeTeamNameAsync(TeamDto TeamDto, string Name);
        Task<bool> UpdateTeamAsync(TeamDto newTeamDto);
        Task<IEnumerable<WorkItemDto>> SearchUsersWorkItems(int userid, string search);
        #endregion

        #region WorkItemDto functionality
        public Task<int> CreateWorkItemAsync(WorkItemDto WorkItemDto);
        public Task<IEnumerable<WorkItemDto>> GetAllWorkItemsAsync();
        public Task<WorkItemDto> GetWorkItemByIdAsync(int id);
        public Task<bool> UpdateWorkItemAsync(WorkItemDto entity);
        public Task<bool> UpdateWorkItemConcurrentlyAsync(WorkItemDto entity);
        public Task<bool> DeleteWorkItemAsync(int id);
        public Task<IEnumerable<WorkItemDto>> GetWorkItemsByUserIdAsync(int id); // for now we get work items from all projects
        public Task<IEnumerable<WorkItemDto>> GetWorkItemsByProjectAsync(int projectId);
        public Task<IEnumerable<UserDto>> GetUsersByWorkItemAsync(int workItemId);
        public Task<bool> AssignUserToWorkItemAsync(int id, UserDto user);
        public Task<bool> UnassignUserFromWorkItemAsync(int id, UserDto user);


        #endregion
    }
}