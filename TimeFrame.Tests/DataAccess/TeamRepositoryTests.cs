using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeFrameLib.Models;
using TimeFrameLib.Repositories;



namespace TimeFrame.Tests.DataAccess
{
    public class TeamRepositoryTests
    {
        private User _user;
        private User _user2;
        private List<User> ListOfMembers;
        private Team _team;
        private Team _newTeam;
        private TeamRepository _teamrepository;
        private UserRepository _userRepository;
        private string _password;
        private string _password2;


        [SetUp]
        public async Task Setup()
        {
            _teamrepository = new TeamRepository(Configuration.CONNECTION_STRING);
            _userRepository = new UserRepository(Configuration.CONNECTION_STRING);
            await CreateNewTeamAsync();
        }

        [TearDown]
        public async Task CleanUp()
        {
            await new TeamRepository(Configuration.CONNECTION_STRING).DeleteAsync(_team.Id);
            await new UserRepository(Configuration.CONNECTION_STRING).DeleteAsync(_user.Id);
            await new UserRepository(Configuration.CONNECTION_STRING).DeleteAsync(_user2.Id);
        }

        private async Task CreateNewTeamAsync() {
            
            _user = new() { Email = "restless1@gmail.com", FirstName = "Mark", LastName = "Rajkovic" };
            _user2 = new() { Email = "mark.rajkovic1@gmail.com", FirstName = "Mark2", LastName = "Rajkovic2" };
            _password = "password123";
            _password2 = "abcd1234";
            _user.Id = await _userRepository.CreateAsync(_user, _password);
            _user2.Id = await _userRepository.CreateAsync(_user2, _password);
            ListOfMembers = new() { _user };
            _team = new() { TeamName = "Me and I", TeamMembers = ListOfMembers };
            ListOfMembers = new() { _user2 };
            _newTeam = new() { TeamName = "I am a new team", TeamMembers = ListOfMembers };
            _team.Id = await _teamrepository.CreateAsync(_team);
        }
        [Test]
        public void CreateTeam(){
            Assert.IsTrue(_team.Id > 0, "Failed to create team or magically the id is smaller than zero. ");        
        }

        [Test]
        public async Task  GetAllAsync()
        {
            IEnumerable<Team> teams = await _teamrepository.GetAllAsync();
            Assert.NotNull(teams,"Collection was null.");

        
        }
        [Test]
        public async Task GetByIdAsync() 
        {
            Team team = await _teamrepository.GetByIDAsync(_team.Id);
            Assert.IsTrue(team.Id==_team.Id);
        }
        [Test]
        public async Task GetByUserAsync() 
        {
            Assert.IsTrue((await _teamrepository.GetByUserIdAsync(_user.Id)).Count>0);
        
        
        }
        [Test]
        public async Task RemoveUserAsync() 
        {
            Assert.IsTrue(await _teamrepository.RemoveUserAsync(_user.Id,_team.Id));
        }
        [Test]
        public async Task ChangeNameAsync() {
            Assert.IsTrue(await _teamrepository.ChangeNameAsync(_team, "HEHEXD"));
        }
  /*      public async Task CreateUsers() {
            bool result1 = await _teamrepository.AddUserToTeamAsync(_team.Id, _user.Id);
            bool result2 = await _teamrepository.AddUserToTeamAsync(_team.Id, _user2.Id);
            Assert.IsTrue(result1 && result2);
        }
  */
        [Test]
        public async Task GetByTeamAsync()
        {
            //arrange
            int expectedNumberOfUsers = 1;
            //act
            List<User> users = await _teamrepository.GetByTeamAsync(_team);
            //assert
            Assert.AreEqual(expectedNumberOfUsers, users.Count, "The number of returned users were not the same.");
        }

        [Test]
        public async Task UpdateTeamAsync()
        {
            //arrange
            _newTeam.Id = _team.Id;
            //act
            bool result = await _teamrepository.UpdateTeamAsync(_newTeam);
            //assert
            Assert.IsTrue(result, "Team was not updated");
        }
    }

}
