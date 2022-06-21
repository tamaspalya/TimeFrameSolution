using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameLib.Repositories
{
    public interface ITeamRepository
    {
        Task<int> CreateAsync(Team team);
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team> GetByIDAsync(int Id);
        Task<bool> DeleteAsync(int Id);
        Task<bool> AddUserAsync(User user, Team team);
        Task<bool> RemoveUserAsync(int userId, int teamId);
        Task<List<Team>> GetByUserIdAsync(int userId);
        Task<List<User>> GetByTeamAsync(Team team);
        Task<bool> ChangeNameAsync(Team team, string Name);
        Task<bool> UpdateTeamAsync(Team newTeam);

    }
}
