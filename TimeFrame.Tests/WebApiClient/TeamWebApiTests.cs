using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFrameWebAPIClient;
using TimeFrameWebAPIClient.DTOs;

namespace TimeFrame.Tests.WebApiClient
{
    
    internal class TeamWebApiTests
    {
        ITimeFrameApiClient _client;
        TeamDto _team=new TeamDto() { TeamName="ApiClientTestTeam"};
        RegisterDto _newuser1 = new RegisterDto() { FirstName = "Test", LastName = "User1", Email = "testuser1@ucn.dk", Password = "1234" };
        RegisterDto _newuser2 = new RegisterDto() { FirstName = "Test", LastName = "User2", Email = "testuser2@ucn.dk", Password = "1234" };
        int _userid1;
        int _userid2;
        [SetUp]
        public async Task Setup()
        {
            _client = new TimeFrameApiClient(Configuration.LOCAL_URI);
            #region SetupUsers
            _userid1 = await _client.CreateUserAsync(_newuser1);
            _userid2 = await _client.CreateUserAsync(_newuser2);
            UserDto user1 = await _client.GetUserByIdAsync(_userid1);
            List < UserDto > users = new List<UserDto>() { user1};
            #endregion
            _team.TeamMembers = users;
            _team.Id = await _client.CreateTeamAsync(_team);
        }
        [TearDown]
        public async Task TearDown()
        {
            await _client.DeleteUserAsync(_userid1);
            await _client.DeleteUserAsync(_userid2);
            await _client.DeleteTeamAsync(_team.Id);
        }
        [Test]
        public async Task GetAllTeamsAsync()
        {
            //arrange
            //act
            IEnumerable<TeamDto> teams = await _client.GetAllTeamsAsync();
            //assert
            Assert.IsNotNull(teams);
        }
        [Test]
        public async Task GetTeamByIdAsync()
        {
            //arrange
            //act
            TeamDto team = await _client.GetTeamByIdAsync(_team.Id);
            //assert
            Assert.IsNotNull(team);
        }
        [Test]
        public async Task AddUserToTeamAsync()
        {
            //arrange
            UserDto user2 = await _client.GetUserByIdAsync(_userid2);
            //act 
            bool success =await _client.AddUserToTeamAsync(user2, _team);
            //assert
            Assert.IsTrue(success);
        }
        [Test]
        public async Task RemoveUserFromTeamAsync()
        {
            //arrange
            UserDto user1 = await _client.GetUserByIdAsync(_userid1);
            //act 
            bool success = await _client.RemoveUserFromTeamAsync(user1, _team);
            //assert
            Assert.IsTrue(success);
        }
        [Test]
        public async Task GetTeamsFromUserIdAsync()
        {
            //arrange
            //act
            IEnumerable<TeamDto> teams = await _client.GetTeamsFromUserIdAsync(_userid1);
            //assert
            Assert.IsNotNull(teams);
        }
        [Test]
        public async Task GetUsersByTeamAsync()
        {
            //arrange
            //act
            IEnumerable<UserDto> users = await _client.GetUsersByTeamAsync(_team);
            //assert
            Assert.IsNotNull(users);
        }
        [Test]
        public async Task ChangeTeamNameAsync() 
        {
            //arrange
            string TeamName = "newname";
            //act
            bool success = await _client.ChangeTeamNameAsync(_team,TeamName);
            //assert
            Assert.IsTrue(success);
        }
        [Test]
        public async Task UpdateTeamAsync()
        {
            //arrange
            TeamDto newteam = new TeamDto() { Id = _team.Id, TeamName = "newteam", TeamMembers = new List<UserDto>() { await _client.GetUserByIdAsync(_userid2) } };
            //act
            bool success = await _client.UpdateTeamAsync(newteam);
            //assert
            Assert.IsTrue(success);
        }

    }
}
/*
 * Task<int> CreateTeamAsync(TeamDto TeamDto);
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task<TeamDto> GetTeamByIdAsync(int Id);
        Task<bool> DeleteTeamAsync(int Id);
        Task<bool> AddUserToTeamAsync(UserDto user, TeamDto TeamDto);
        Task<bool> RemoveUserFromTeamAsync(UserDto user, TeamDto TeamDto);
        Task<IEnumerable<TeamDto>> GetTeamsFromUserIdAsync(int userId);
        Task<IEnumerable<UserDto>> GetUsersByTeamAsync(TeamDto TeamDto);
        Task<bool> ChangeTeamNameAsync(TeamDto TeamDto, string Name);
        Task<bool> UpdateTeamAsync(TeamDto newTeamDto);
*/