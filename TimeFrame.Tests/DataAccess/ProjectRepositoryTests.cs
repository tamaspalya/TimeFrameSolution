using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFrameLib.Models;
using TimeFrameLib.Repositories;

namespace TimeFrame.Tests.DataAccess
{
    class ProjectRepositoryTests
    {
        private IProjectRepository _projectRepository;
        private ITeamRepository _teamRepository;

        private Project _project;
        private Team _team;

        [SetUp]
        public async Task SetUp()
        {
            _projectRepository = new ProjectRepository(Configuration.CONNECTION_STRING);
            _teamRepository = new TeamRepository(Configuration.CONNECTION_STRING);

            #region Team creation
            _team = new() { TeamName = "Test team", TeamMembers = new() };
            _team.Id = await _teamRepository.CreateAsync(_team);
            #endregion 

            #region Project creation
            _project = new() { ProjectName = "Test project", TeamID = _team.Id, ProjectDescription = "blabla" };
            _project.ID = await _projectRepository.CreateProjectAsync(_project);
            #endregion
        }

        [TearDown]
        public async Task CleanUp()
        {
            await _teamRepository.DeleteAsync(_team.Id);
        }

        [Test]
        public void CreateProject()
        {
            //arrange
            int id = _project.ID;
            //act
            //done in SetUp
            //assert
            Assert.IsTrue(id > 0, "Failed to create a team.");
        }

        [Test]
        public async Task GetAllProjectsAsync()
        {
            //arrange
            //act
            IEnumerable<Project> projects = await _projectRepository.GetAllAsync();
            //assert
            Assert.NotNull(projects, "There were no projects returned.");
            
        }

        [Test]
        public async Task GetProjectByIdAsync()
        {
            //arrange
            int id = _project.ID;
            //act
            Project project = await _projectRepository.GetProjectByIdAsync(id);
            //assert
            Assert.IsNotNull(project, $"There was no returned project with id: {id}");
        }

        [Test]
        public async Task GetProjectsByTeamIdAsync()
        {
            //arrange
            int expectedNumberOfProjects = 1;
            int teamId = _team.Id;
            //act
            IEnumerable<Project> projects = await _projectRepository.GetProjectsByTeamIdAsync(teamId);
            //assert
            Assert.AreEqual(expectedNumberOfProjects, projects.ToList().Count, "The returned number of projects associated with the given team was incorrect.");
        }
    }
}
