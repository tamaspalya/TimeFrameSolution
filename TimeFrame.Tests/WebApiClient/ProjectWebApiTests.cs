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
    class ProjectWebApiTests
    {
        ITimeFrameApiClient _client;
        ProjectDto _project;
        TeamDto _team;

        [SetUp]
        public async Task SetUp()
        {
            _client = new TimeFrameApiClient(Configuration.LOCAL_URI);

            #region Create team
            _team = new() { TeamName = "Test", TeamMembers = new()};
            _team.Id = await _client.CreateTeamAsync(_team);
            #endregion

            #region Create project
            _project = new() { ProjectName = "Test", ProjectDescription = "nothing", IsPrivate = true, TeamID = _team.Id};
            _project.ID = await _client.CreateProjectAsync(_project);
            #endregion
        }

        [TearDown]
        public async Task CleanUp()
        {
            await _client.DeleteProjectAsync(_project.ID);
            await _client.DeleteTeamAsync(_team.Id);
        }

        [Test]
        public void CreateProject()
        {
            //arrange
            int id = _project.ID;
            //act
            //done in SetUp
            //assert
            Assert.IsTrue(id > 0, "Error creating a project through the web api client.");
        }

        [Test]
        public async Task GetAllProjectsAsync()
        {
            //arrange
            //act
            IEnumerable<ProjectDto> projects = await _client.GetAllProjectsAsync();
            //assert
            Assert.IsNotNull(projects, "Failed to retrieve a list of projects");
        }
        
        [Test]
        public async Task GetByIdAsync()
        {
            //arrange
            int id = _project.ID;
            //act
            ProjectDto project = await _client.GetProjectByIdAsync(id);
            //assert
            Assert.IsNotNull(project, $"The was no returned project with id = {id}");
        }

        [Test]
        public async Task GetProjectsByTeamIdAsync()
        {
            //arrange
            int numberOfExpectedProjects = 1;
            int id = _team.Id;
            //act
            IEnumerable<ProjectDto> projects = await _client.GetProjectsByTeamIdAsync(id);
            //assert
            Assert.AreEqual(numberOfExpectedProjects, projects.ToList().Count,"The number of projects associated with the given team was incorrect.");
        }
    }
}
