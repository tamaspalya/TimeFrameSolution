using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TimeFrameWebAPIClient.DTOs;

namespace TimeFrameWebAPIClient
{
    public class TimeFrameApiClient : ITimeFrameApiClient
    {
        private RestClient _restClient;
        public TimeFrameApiClient(string uri) => _restClient = new RestClient(new Uri(uri));

        #region User functionality

        public async Task<int> CreateUserAsync(RegisterDto entity)
        {
            var response = await _restClient.RequestAsync<int>(Method.POST, "users", entity);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error creating user with email={entity.Email}. Message was {response.Content}");
            }
            return response.Data;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var response = await _restClient.RequestAsync<IEnumerable<UserDto>>(Method.GET, "users/all", null);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving all users. Message was {response.Content}");
            }
            return response.Data;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var response = await _restClient.RequestAsync<UserDto>(Method.GET, $"users/{id}");

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving all users. Message was {response.Content}");
            }
            return response.Data;
        }

        public async Task<bool> UpdateUserAsync(UserDto entity)
        {
            var response = await _restClient.RequestAsync(Method.PUT, $"users/{entity.Id}", entity);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error updating user with email={entity.Email}. Message was {response.Content}");
            }

        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _restClient.RequestAsync(Method.DELETE, $"users/{id}", null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error deleting user with id={id}. Message was {response.Content}");
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var response = await _restClient.RequestAsync<IEnumerable<UserDto>>(Method.GET, $"users?email={email}");

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving all users. Message was {response.Content}");
            }
            return response.Data.FirstOrDefault();
        }

        public async Task<bool> UpdateUserPasswordAsync(int userid, string oldpassword, string newpassword)
        {
            throw new Exception("Not Implemented");
            /*var response = await _restClient.RequestAsync(Method.PUT, $"users/{user.Id}/Password", user);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error updating password for user with email={user.Email}. Message was {response.Content}");
            }
            */
        }

        public async Task<int> LoginAsync(DTOs.LoginDto loginInfo)
        {
            var response = await _restClient.RequestAsync<int>(Method.POST, $"logins", loginInfo);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error logging in for user with email={loginInfo.Email}. Message was {response.Content}");
            }
            return response.Data;   
        }

        #endregion

        #region Project functionality
        public async Task<int> CreateProjectAsync(Object entity)  //TODO: Change object to ProjectDto after ProjectDto class is added 
        {
            var response = await _restClient.RequestAsync<int>(Method.POST, "project", entity);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error creating the project. Message was {response.Content}");
            }
            return response.Data;
        }
        public async Task<int> CreateProjectAsync(ProjectDto entity)
        {
            var response = await _restClient.RequestAsync<int>(Method.POST, "projects", entity);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error creating project with Id={entity.ID}. Message was {response.Content}");
            }
            return response.Data;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var response = await _restClient.RequestAsync<IEnumerable<ProjectDto>>(Method.GET, $"projects/");
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving all projects. Message was {response.Content}");
            }
            return response.Data;
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var response = await _restClient.RequestAsync<ProjectDto>(Method.GET, $"projects/{id}");

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving projects. Message was {response.Content}");
            }
            return response.Data;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByTeamIdAsync(int teamId)
        {
            var response = await _restClient.RequestAsync<IEnumerable<ProjectDto>>(Method.GET, $"projects/team/{teamId}");

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving projects. Message was {response.Content}");
            }
            return response.Data;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var response = await _restClient.RequestAsync<bool>(Method.DELETE, $"projects/{id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            throw new Exception($"Error deleting a project with id: {id}. Message was: {response.Content}");
            
        }
        #endregion

        #region Team functionality
        public async Task<int> CreateTeamAsync(TeamDto TeamDto)
        {
            var response = await _restClient.RequestAsync<int>(Method.POST, "teams", TeamDto);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error creating a team with name: {TeamDto.TeamName}. Message was: {response.Content}");
            }
            return response.Data;
        }

        public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
        {
            var response = await _restClient.RequestAsync<IEnumerable<TeamDto>>(Method.GET, "teams", null);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error getting all teams. Message was: {response.Content}");
            }
            return response.Data;
        }

        public async Task<TeamDto> GetTeamByIdAsync(int Id)
        {
            var response = await _restClient.RequestAsync<TeamDto>(Method.GET, $"teams/{Id}");
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error getting team with id: {Id}. Message was: {response.Content}");
            }
            return response.Data;
        }

        public async Task<bool> DeleteTeamAsync(int Id)
        {
            var response = await _restClient.RequestAsync<bool>(Method.DELETE, $"teams/{Id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error deleting a team with id: {Id}. Message was: {response.Content}");
            }

        }

        public async Task<bool> AddUserToTeamAsync(UserDto user, TeamDto teamDto)
        {
            teamDto.TeamMembers.Add(user);
            var response = await _restClient.RequestAsync<bool>(Method.PUT, $"teams", teamDto);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error adding a user to team. Message was: {response.Content}");
            }
        }

        public async Task<bool> RemoveUserFromTeamAsync(UserDto user, TeamDto teamDto)
        {
            teamDto.TeamMembers.Remove(user);
            var response = await _restClient.RequestAsync<bool>(Method.PUT, $"teams/{teamDto.Id}/{user.Id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            } else
            {
                throw new Exception($"Error removing a user from team. Message was: {response.Content}");
            }
        }

        public async Task<IEnumerable<TeamDto>> GetTeamsFromUserIdAsync(int userId)
        {
            var response = await _restClient.RequestAsync<IEnumerable<TeamDto>>(Method.GET, $"teams/users/{userId}");
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error getting teams by user id: {userId}. Message was: {response.Content}");
            }
            return response.Data;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByTeamAsync(TeamDto teamDto)
        {
            var response = await _restClient.RequestAsync<IEnumerable<UserDto>>(Method.GET, $"teams/{teamDto.Id}/users");
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error getting users by team: {teamDto.Id}. Message was: {response.Content}");
            }
            return response.Data;
        }

        public async Task<bool> ChangeTeamNameAsync(TeamDto teamDto, string Name)
        {
            teamDto.TeamName = Name;
            var response = await _restClient.RequestAsync<bool>(Method.PUT, $"teams",teamDto);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error changing a name of team with id: {teamDto.Id}. Message was: {response.Content}");
            }
        }

        public async Task<bool> UpdateTeamAsync(TeamDto newTeamDto)
        {
            var response = await _restClient.RequestAsync<bool>(Method.PUT, $"teams", newTeamDto);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                throw new Exception($"Error updating team with id: {newTeamDto.Id}. Message was: {response.Content}");
            }
        }
        #endregion

        #region WorkItem functionality
        public async Task<int> CreateWorkItemAsync(WorkItemDto workItem)
        {
            var response = await _restClient.RequestAsync<int>(Method.POST, "WorkItems", workItem);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error creating WorkItem, message was: {response.Content}");

            }
        }

        public async Task<IEnumerable<WorkItemDto>> GetAllWorkItemsAsync()
        {
            var response = await _restClient.RequestAsync<IEnumerable<WorkItemDto>>(Method.GET, "WorkItems");
            if (response.IsSuccessful) {
                return response.Data;

            }
            else
            {
                throw new Exception($"Error getting all work items, message was: {response.Content}");
            }
        }

        public async Task<WorkItemDto> GetWorkItemByIdAsync(int id)
        {
            var response = await _restClient.RequestAsync<WorkItemDto>(Method.GET, $"WorkItems/{id}");
            if (response.IsSuccessful) {
                return response.Data;
            } 
            else
            {
                throw new Exception($"Error getting workitem with id {id}, message was: {response.Content}");
            }
        }

        public async Task<bool> UpdateWorkItemAsync(WorkItemDto entity)
        {
            var response = await _restClient.RequestAsync(Method.PUT, $"WorkItems/{entity.Id}", entity);
            if (response.StatusCode== HttpStatusCode.OK)
            {
                return true;

            }
            else
            {
                throw new Exception($"Failed to update WorkItem with id {entity.Id}, message was: {response.Content}");
            }
        }

        public async Task<bool> UpdateWorkItemConcurrentlyAsync(WorkItemDto entity)
        {
            var response = await _restClient.RequestAsync(Method.PUT, $"WorkItems/Update/{entity.Id}", entity);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteWorkItemAsync(int id)
        {
            var response = await _restClient.RequestAsync<bool>(Method.DELETE, $"WorkItems/{id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Failed to delete WorkItem with id {id}, message was {response.Content}");
            }
        }

        public async Task<IEnumerable<WorkItemDto>> GetWorkItemsByUserIdAsync(int userid)
        {
            var response = await _restClient.RequestAsync<IEnumerable<WorkItemDto>>(Method.GET, $"WorkItems/PeopleAssigned/{userid}/Tasks");
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Failed to get WorkItems by user with Id {userid}, message was: {response.Content}");

            }
        }
        public async Task<IEnumerable<WorkItemDto>> GetWorkItemsByProjectAsync(int projectId)
        {
            var response = await _restClient.RequestAsync<IEnumerable<WorkItemDto>>(Method.GET, $"WorkItems/Project/{projectId}");
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Failed to get WorkItems by Project with Id {projectId}, message was: {response.Content}");
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersByWorkItemAsync(int workItemId)
        {
            var response = await _restClient.RequestAsync<IEnumerable<UserDto>>(Method.GET, $"WorkItems/PeopleAssigned/{workItemId}");
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else 
            {
                throw new Exception($"Failed to get users assigned to WorkItem with id {workItemId}, message was: {response.Content}");    
            }

        }

        public async Task<bool> AssignUserToWorkItemAsync(int workItemId, UserDto user)
        {
            var response = await _restClient.RequestAsync<bool>(Method.PUT, $"WorkItems/{workItemId}/Assign", user);
            if (response.StatusCode == HttpStatusCode.OK) 
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Failed to assign user {user.Email} to task with Id{workItemId}, message was: {response.Content}");
            }
        }

        public async Task<bool> UnassignUserFromWorkItemAsync(int workItemId, UserDto user)
        {
            var response = await _restClient.RequestAsync<bool>(Method.PUT, $"WorkItems/{workItemId}/Unassign", user);
            if (response.StatusCode == HttpStatusCode.OK) {
                return response.Data;
            }
            else
            {
                throw new Exception($"Failed to unnasign user {user.Email} from task with Id {workItemId}, message was {response.Content}");
            }
        }
        public async Task<IEnumerable<WorkItemDto>> SearchUsersWorkItems(int userid, string search)
        {
            var response = await _restClient.RequestAsync<IEnumerable<WorkItemDto>>(Method.GET, $"WorkItems/{userid}/search={search}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Failed to find work items with content {search} assigned to user {userid}. Message was {response.Content}");
            }
        }

        
        #endregion

    }
}
