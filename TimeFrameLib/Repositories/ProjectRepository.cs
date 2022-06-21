using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameLib.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(string connectionstring) : base(connectionstring)
        {
        }

        public async Task<int> CreateProjectAsync(Project entity)
        {
            try
            {
                var query = "INSERT INTO Project(TeamID, ProjectName, ProjectDescription) OUTPUT INSERTED.Id VALUES (@TeamID, @ProjectName, @ProjectDescription)";
                using var connection = CreateConnection();
                return await connection.QuerySingleAsync<int>(query, new { TeamID = entity.TeamID, ProjectName = entity.ProjectName, ProjectDescription = entity.ProjectDescription });  
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating project: '{ex.Message}'.", ex);
            }
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            try
            {
                var query = "DELETE FROM [Project] WHERE Id = @Id";
                using var connection = CreateConnection();
                return await connection.ExecuteAsync(query, new { id }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating project: '{ex.Message}'.", ex);
            }
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            try
            {
                var query = "SELECT * FROM [Project]";
                using var connection = CreateConnection();
                return (await connection.QueryAsync<Project>(query)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all Users: '{ex.Message}'.", ex);
            }
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            try
            {
                var query = "SELECT * FROM Project WHERE Id=@Id";
                using var connection = CreateConnection();
                var project = (await connection.QueryAsync<Project>(query, new { id })).First();
                return project;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting a project by id: '{ex.Message}'.", ex);
            }
        }

        public async Task<IEnumerable<Project>> GetProjectsByTeamIdAsync(int teamId)
        {
            try
            {
                var query = "SELECT * FROM Project WHERE TeamId=@TeamId";
                using var connection = CreateConnection();
                var projects = (await connection.QueryAsync<Project>(query, new { teamId })).ToList();
                return projects;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting a project by id: '{ex.Message}'.", ex);
            }
        }
    }
}
