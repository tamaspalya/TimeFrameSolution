using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeFrameLib.Models;
using TimeFrameLib.Repositories;
using System.Linq;
using System;

namespace TimeFrame.Tests.DataAccess
{
    public class WorkItemRepositoryTests
    {
        public IWorkItemRepository _workItemRepository;

        public WorkItem _workItem;
        string _workItemName = "WORK ITEM !!!";
        int _projectId = 79;
        User _user;
        Project _project;

        [SetUp]
        public async Task Setup()
        {
            #region Model creation
            _user = new User() { Id = 588 };
            _project = new Project() { ID = _projectId };
            _workItem = new WorkItem() { WorkItemName = _workItemName, ProjectId = _projectId, StartDate = new DateTime(2021, 11, 25), EndDate = new DateTime(2021, 12, 1), Description = "important this is" };
            #endregion

            _workItemRepository = new WorkItemRepository(Configuration.CONNECTION_STRING);

            _workItem.Id = await _workItemRepository.CreateWorkItemAsync(_workItem);
            //await _workItemRepository.AssignUserAsync(_workItem.Id, _user);
        }

        [TearDown]
        public async Task CleanUp()
        {
            await _workItemRepository.DeleteAsync(_workItem.Id);
        }

        [Test]
        public void CreateWorkItem()
        {
            //arrange - done in Setup
            //act - done in Setup
            //assert
            Assert.True(_workItem.Id > 0, "Creating work item was unsuccessful, the returned id was smaller than 0.");
            Assert.AreEqual(_workItem.WorkItemName, _workItemName, "The work item name was different.");
        }

        [Test]
        public async Task GetAllWorkItemsAsync()
        {
            //arrange - done is Setup
            //act
            List<WorkItem> workItems = Enumerable.ToList(await _workItemRepository.GetAllAsync());
            //assert
            Assert.NotNull(workItems, "The retrieved collection was null.");
            Assert.IsTrue(workItems.Count > 0, "The number of returned work items was incorrect");
        }

        [Test]
        public async Task GetWorkItemByIdAsync()
        {
            //arrange - done in setup
            //act
            WorkItem retrievedWorkItem = await _workItemRepository.GetByIdAsync(_workItem.Id);
            //assert
            Assert.NotNull(retrievedWorkItem, "There was no work item retrieved.");
            Assert.AreEqual(_workItem.Id, retrievedWorkItem.Id, "The work item id retrieved did not match the expected.");
        }

        [Test]
        public async Task UpdateWorkItemAsync()
        {
            //arrange
            WorkItem updatedWorkItem = new WorkItem() {Id = _workItem.Id, WorkItemName = "I have been updated", ProjectId = _project.ID, StartDate = new DateTime(2021, 10, 25), EndDate = new DateTime(2021, 11, 1), Milestone = false, Status = Status.NotSet, Priority = Priority.Low, Description = ""};
            //act
            bool result = await _workItemRepository.UpdateAsync(_workItem.Id, updatedWorkItem);
            //assert
            Assert.IsTrue(result, $"Updating work item with id: {_workItem.Id} failed.");
        }

      

        /* Moved to setup method, because it was required in other tests
        [Test]
        public async Task AssignUserToWorkItemAsync()
        {
            //arrange - done in setup
            //act
            bool result = await _workItemRepository.AssignUserAsync(_workItem.Id, _user);
            //assert
            Assert.IsTrue(result, $"Assigning user with id: {_user.Id} to work item with id: {_workItem.Id} was unsuccessful.");
        }
        */

        /*
        [Test]
        public async Task UnassignUserFromWorkItemAsync()
        {
            //arrange - done in setup
            //act
            bool result = await _workItemRepository.UnassignUserAsync(_workItem.Id, _user);
            //assert
            Assert.IsTrue(result, $"Unassigning user with id: {_user.Id} from work item with id: {_workItem.Id} was unsuccessful.");
        }
        */

        /*
        [Test]
        public async Task GetWorkItemByUserIdAsync()
        {
            //arrange - done in setup
            //funny comment
            //act
            IEnumerable<WorkItem> workItems = await _workItemRepository.GetByUserIdAsync(_user.Id);
            //assert
            Assert.IsNotNull(workItems, "The returned collection was null.");
            Assert.IsTrue(0 < workItems.ToList().Count, "The returned collection was empty.");
        }
        */

        [Test]
        public async Task UpdateWorkItemConcurrentlyAsync()
        {
            //arrange

            //act
            WorkItem workItemChangesNumberOne = await _workItemRepository.GetByIdAsync(_workItem.Id); //first user reads
            workItemChangesNumberOne.WorkItemName = "New name"; //first user modifies
            WorkItem workItemChangesNumberTwo = await _workItemRepository.GetByIdAsync(_workItem.Id); //second user reads
            bool firstUpdateResult = await _workItemRepository.UpdateConcurrentlyAsync(_workItem.Id, workItemChangesNumberOne); //first user updates
            workItemChangesNumberTwo.Description = "I could write pages, but I do not like novels."; //second user modifies
            bool secondUpdateResult = await _workItemRepository.UpdateConcurrentlyAsync(_workItem.Id, workItemChangesNumberTwo); //second user updates
            //assert

            Assert.IsTrue(firstUpdateResult, "First update failed. User one failed to update work item.");
            Assert.IsFalse(secondUpdateResult, "Second update was successful. User two overwrote the changes of user one.");
        }

        

    }

}
