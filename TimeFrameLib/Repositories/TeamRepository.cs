using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameLib.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        private string connectionstring;
        
        public TeamRepository(string connectionstring) : base(connectionstring) { }
        
        public async Task<bool> AddUserAsync(User user, Team team)
        {
            try
            {
                using var connection = CreateConnection();
                var query2 = "INSERT INTO [UserTeamTable] (UserID, TeamID) VALUES (@UserID, @TeamID)";
                return await connection.ExecuteAsync(query2, new { UserID = user.Id, TeamID = team.Id }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding a user to a team: '{ex.Message}'.", ex);
            }
        }

        public async Task<int> CreateAsync(Team team)
        {
            try
            {
                var query = "INSERT INTO [Team] (TeamName) OUTPUT INSERTED.Id VALUES (@TeamName);";
                using var connection = CreateConnection();

                team.Id= await connection.QuerySingleAsync<int>(query, new { TeamName = team.TeamName });
                for (int i = 0; i < team.TeamMembers.Count; i++)
                {
                    await AddUserAsync(team.TeamMembers[i], team);
                }

                return team.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating a team: '{ex.Message}'.", ex);
            }




        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var query = "DELETE FROM [Team] WHERE Id=@Id";
                using var connection = CreateConnection();
                return await connection.ExecuteAsync(query, new { id }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting a team: '{ex.Message}'.", ex);
            }

        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var query = "SELECT * FROM Team";
                List<Team> TeamList=(await connection.QueryAsync<Team>(query)).ToList();
                for (int i = 0; i < TeamList.Count; i++)
                {
                    TeamList[i].TeamMembers = await GetByTeamAsync(TeamList[i]);
                }
                return TeamList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all teams: '{ex.Message}'.", ex);
            }
        }

        public async Task<Team> GetByIDAsync(int id)
        {

            try
            {
                var query = "SELECT * FROM Team WHERE Id=@Id";
                using var connection = CreateConnection();
                var team = await connection.QuerySingleAsync<Team>(query, new { id });
                team.TeamMembers = await GetByTeamAsync(team);
                return team;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting a team by id: '{ex.Message}'.", ex);
            }

        }
        public async Task<List<Team>> GetByUserIdAsync(int userid)
        {
            try
            {
                var query = "SELECT [TeamID] FROM [UserTeamTable] WHERE [UserID]=@UserId";
                using var connection = CreateConnection();
                List<int> teamIds = (await connection.QueryAsync<int>(query, new { UserId = userid })).ToList();
                List<Team> TeamList = new List<Team>();
                for (int i = 0; i < teamIds.Count; i++)
                {
                    int teamId = teamIds[i];
                    TeamList.Add(await GetByIDAsync(teamId));



                }


                return TeamList;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting a team by user: '{ex.Message}'.", ex);
            }
        }
        public async Task<List<User>> GetByTeamAsync(Team team)
        {
            try
            {
                var query = "SELECT [UserID] FROM [UserTeamTable] WHERE [TeamID]=@TeamId";
                using var connection = CreateConnection();
                List <int> userIds=(await connection.QueryAsync<int>(query, new { TeamId=team.Id})).ToList();
                List<User> teamMembers = new List<User>();
                UserRepository userRepository = new UserRepository(Configuration.CONNECTION_STRING);
                for (int i = 0; i < userIds.Count; i++)
                {
                    int userId = userIds[i];
                    teamMembers.Add(await userRepository.GetByIdAsync(userId));
                }
                return teamMembers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting users by team: '{ex.Message}'.", ex);
            }
        }


        public async Task<bool> RemoveUserAsync(int userId, int teamId)
        {
            try
            {
                var query = "DELETE FROM [UserTeamTable] WHERE [UserID]=@userId AND [TeamID]=@teamId";
                using var connection = CreateConnection();
                return await connection.ExecuteAsync(query, new { userId = userId, teamId = teamId }) > 0;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing a user from team: '{ex.Message}'.", ex);
            }
        }

        public async Task<bool> ChangeNameAsync(Team team, string Name)
        {
            try
            {
                var query = "UPDATE [Team] SET [TeamName]=@teamName WHERE [Id]=@teamId";
                using var connection = CreateConnection();
                return await connection.ExecuteAsync(query, new { teamName = Name, teamId = team.Id }) > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateTeamAsync(Team newTeam)
        {
            try
            {
                var oldTeam = await GetByIDAsync(newTeam.Id);
                foreach (var oldUser in oldTeam.TeamMembers)
                {
                    bool noOldUser = true;
                    foreach (var newUser in newTeam.TeamMembers)
                    {
                        if (oldUser.Id == newUser.Id)
                        {
                            noOldUser = false;
                        }
                    }
                    if (noOldUser)
                    {
                        await RemoveUserAsync(oldUser.Id, oldTeam.Id);
                    }
                }

                foreach (var newUser in newTeam.TeamMembers)
                {
                    bool noNewUser = true;
                    foreach (var oldUser in oldTeam.TeamMembers)
                    {
                        if (newUser.Id == oldUser.Id)
                        {
                            noNewUser = false;
                        }
                    }
                    if (noNewUser)
                    {
                        await AddUserAsync(newUser, newTeam);
                    }
                }

                return  await ChangeNameAsync(newTeam, newTeam.TeamName);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
/*
  
 */
