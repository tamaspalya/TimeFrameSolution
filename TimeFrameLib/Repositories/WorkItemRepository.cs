using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TimeFrameLib.Models;

namespace TimeFrameLib.Repositories
{
    public class WorkItemRepository : BaseRepository, IWorkItemRepository
    {
        private string connectionstring;

        public WorkItemRepository(string connectionstring) : base(connectionstring) { }

        public async Task<int> CreateWorkItemAsync(WorkItem workItem)
        {
            try
            {
                var query = "INSERT INTO [WorkItem] (WorkItemName, ProjectId, Description,  StartDate, EndDate) OUTPUT INSERTED.Id VALUES (@WorkItemName, @ProjectId, @Description, @StartDate, @EndDate)";
                using var connection = CreateConnection();
                return await connection.QuerySingleAsync<int>(query, new { WorkItemName = workItem.WorkItemName, ProjectId = workItem.ProjectId, Description = workItem.Description, StartDate = workItem.StartDate, EndDate = workItem.EndDate });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding a work item to the project: '{ex.Message}'.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var query = "DELETE FROM [WorkItem] WHERE Id = @Id";
                using var connection = CreateConnection();
                return await connection.ExecuteAsync(query, new { id }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting work item: '{ex.Message}'.", ex);
            }
        }

        public async Task<IEnumerable<WorkItem>> GetAllAsync()
        {
            try
            {
                var query1 = "SELECT * FROM [WorkItem]";
                using var connection = CreateConnection();
                var workItems = (await connection.QueryAsync<WorkItem>(query1)).ToList();

                var query2 = "SELECT UserId FROM [WorkItemUser] WHERE WorkItemId=@Id";
                foreach (var workItem in workItems)
                {
                    var workItemUserIds = (await connection.QueryAsync<int>(query2, new { Id = workItem.Id })).ToList();
                    workItem.PeopleAssignedId = workItemUserIds;
                }

                return workItems;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all work items: '{ex.Message}'.", ex);
            }
        }

        public async Task<WorkItem> GetByIdAsync(int id)
        {
            try
            {
                var query1 = "SELECT * FROM [WorkItem] WHERE Id=@Id";
                using var connection = CreateConnection();  
                var workItem = await connection.QuerySingleAsync<WorkItem>(query1, new { id });

                var query2 = "SELECT UserId FROM [WorkItemUser] WHERE WorkItemId=@Id";
                var workItemUserIds = (await connection.QueryAsync<int>(query2, new { id })).ToList();

                workItem.PeopleAssignedId = workItemUserIds;

                return workItem;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting work item by Id: '{ex.Message}'.", ex);
            }
        }

        public async Task<IEnumerable<WorkItem>> GetByProjectAsync(int projectId)
        {
            try
            {
                var query1 = "SELECT * FROM [WorkItem] WHERE ProjectId = @ProjectId";
                using var connection = CreateConnection();
                var workItems = (await connection.QueryAsync<WorkItem>(query1, new { projectId })).ToList();

                var query2 = "SELECT UserId FROM [WorkItemUser] WHERE WorkItemId=@Id";
                foreach (var workItem in workItems)
                {
                    var workItemUserIds = (await connection.QueryAsync<int>(query2, new { Id = workItem.Id })).ToList();
                    workItem.PeopleAssignedId = workItemUserIds;
                }

                return workItems;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all work items by project ID: '{ex.Message}'.", ex);
            }
        }

        public async Task<IEnumerable<WorkItem>> GetByUserIdAsync(int userId)
        {
            try
            {
                var query = "SELECT * FROM [WorkItem] JOIN [WorkItemUser] ON WorkItemId=Id WHERE UserId=@UserId";
                using var connection = CreateConnection();
                return (await connection.QueryAsync<WorkItem>(query, new { userId })).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all work items by user ID: '{ex.Message}'.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetUsersByWorkItemAsync(int workItemId)
        {
            try
            {
                var query = "SELECT * FROM [User] INNER JOIN [WorkItemUser] ON UserId = Id WHERE WorkItemId=@WorkItemId";
                using var connection = CreateConnection();
                return (await connection.QueryAsync<User>(query, new { workItemId })).ToList();
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all Users by work item ID: '{ex.Message}'.", ex);
            }
        }

        public async Task<bool> AssignUserAsync(int workItemId, User user)
        {
            try
            {
                var query = "INSERT INTO [WorkItemUser] VALUES (@WorkItemId, @UserId)";
                using var connection = CreateConnection();
                return await connection.ExecuteAsync(query, new { workItemId, UserId = user.Id }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error assigning user to a work item: '{ex.Message}'.", ex);
            }
        }

        public async Task<bool> UnassignUserAsync(int workItemId, User user)
        {
            try
            {
                var query = "DELETE FROM [WorkItemUser] WHERE WorkItemId=@WorkItemId AND UserId=@UserId;";
                using var connection = CreateConnection();
                return await connection.ExecuteAsync(query, new { workItemId, UserId = user.Id }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error un-assigning user to work item: '{ex.Message}'.", ex);
            }
        }   

        public async Task<bool> UpdateAsync(int id, WorkItem workItem)
        {
            try
            {
                var query = "UPDATE [WorkItem] SET WorkItemName=@WorkItemName, StartDate=@StartDate,EndDate=@EndDate, Status=@Status, Priority=@Priority,Description=@Description,Milestone=@Milestone WHERE Id=@Id;";
                using var connection = CreateConnection();
                return await connection.ExecuteAsync(query, new
                {
                    workItem.WorkItemName,
                    workItem.StartDate,
                    workItem.EndDate,
                    workItem.Status,
                    workItem.Priority,
                    workItem.Description,
                    workItem.Milestone,
                    workItem.Id,
                }) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating work item: '{ex.Message}'.", ex);
            }
        }

        public async Task<IEnumerable<WorkItem>> SearchUsersWorkItems(int userid, string search)
        {
            try
            {
                var query = "SELECT * FROM [WorkItem] JOIN [WorkItemUser] ON WorkItemId=Id WHERE UserId=@UserId AND (WorkItemName LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR Description LIKE '%' + LTRIM(RTRIM(@search)) + '%')";
                using var connection = CreateConnection();
                return (await connection.QueryAsync<WorkItem>(query, new { UserId = userid, search})).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception($"Error finding work items assigned to user {userid} containing {search}. Message was: '{ex.Message}'.", ex);
            }
        }

        public async Task<bool> UpdateConcurrentlyAsync(int id, WorkItem workItem)
        {
            try
            {
                WorkItem existingWorkItem = await GetByIdAsync(id);
                if (existingWorkItem == null)
                {
                    return false; // update fails since entity is null
                }
                if (Convert.ToBase64String(existingWorkItem.Version) == Convert.ToBase64String(workItem.Version))
                {
                    return await UpdateAsync(workItem.Id, workItem);
                }
                return false; //update fails since entity version has been changed
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating work item concurrently: '{ex.Message}'.", ex);
            }
        }

        public async Task<bool> UpdateConcurrentlyInTransactionAsync(int id, WorkItem workItem)
        {
            try
            {
                using var connection = CreateConnection();
                //in case of transactions, connection MUST be opened
                connection.Open();
                using var transaction = connection.BeginTransaction();

                var getWorkItemQuery = "SELECT * FROM [WorkItem] WHERE Id=@Id";
                var existingWorkItem = await connection.QuerySingleAsync<WorkItem>(getWorkItemQuery, new { id }, transaction);

                if (existingWorkItem == null)
                {
                    //work item not found, roll back and return false
                    transaction.Rollback();
                    return false;
                }

                //var getAssignedUserIds = "SELECT UserId FROM [WorkItemUser] WHERE WorkItemId=@Id";
                //var workItemUserIds = (await connection.QueryAsync<int>(getAssignedUserIds, new { id }, transaction)).ToList();

                
                if (Convert.ToBase64String(existingWorkItem.Version) == Convert.ToBase64String(workItem.Version))
                {
                    var query = "UPDATE [WorkItem] SET WorkItemName=@WorkItemName, StartDate=@StartDate,EndDate=@EndDate, Status=@Status, Priority=@Priority,Description=@Description,Milestone=@Milestone WHERE Id=@Id;";
                    bool result = await connection.ExecuteAsync(query, new
                    {
                        workItem.WorkItemName,
                        workItem.StartDate,
                        workItem.EndDate,
                        workItem.Status,
                        workItem.Priority,
                        workItem.Description,
                        workItem.Milestone,
                        workItem.Id,
                    },
                    transaction) > 0;

                    if (result)
                    {
                        transaction.Commit();
                        return result;
                    }
                }
                transaction.Rollback();
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating work item concurrently: '{ex.Message}'.", ex);
            }
        }
    }
}