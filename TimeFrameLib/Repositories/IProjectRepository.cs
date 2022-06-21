using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameLib.Repositories
{
    public interface IProjectRepository
    {
        //Default CRUD methods for Repository (DAO pattern)
        Task<int> CreateProjectAsync(Project entity);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task<IEnumerable<Project>> GetProjectsByTeamIdAsync(int teamId);
        Task<bool> DeleteProjectAsync(int id);
    }
}
