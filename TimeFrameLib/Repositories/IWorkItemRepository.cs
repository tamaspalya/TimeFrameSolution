using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameLib.Repositories
{
    public interface IWorkItemRepository
    {
        public Task<int> CreateWorkItemAsync(WorkItem workItem);
        public Task<IEnumerable<WorkItem>> GetAllAsync();
        public Task<WorkItem> GetByIdAsync(int id);
        public Task<bool> UpdateAsync(int id, WorkItem entity);
        public Task<bool> UpdateConcurrentlyAsync(int id, WorkItem entity);
        public Task<bool> UpdateConcurrentlyInTransactionAsync(int id, WorkItem entity);
        public Task<bool> DeleteAsync(int id);
        public Task<IEnumerable<WorkItem>> GetByUserIdAsync(int userId);
        public Task<IEnumerable<WorkItem>> GetByProjectAsync(int projectId);
        public Task<IEnumerable<User>> GetUsersByWorkItemAsync(int workItemId);
        public Task<bool> AssignUserAsync(int id, User user);
        public Task<bool> UnassignUserAsync(int id, User user);
        public Task<IEnumerable<WorkItem>> SearchUsersWorkItems(int userid, string search);
    }
}
