using System;
using System.Collections.Generic;

namespace TimeFrameWebAPIClient.DTOs
{
    public class WorkItemDto
    {
        public int ProjectId { get; set; }

        public int Id { get; set; }
        public string WorkItemName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public List<int> PeopleAssignedId { get; set; }
        public List<UserDto> PeopleAssigned { get; set; }
        public string Description { get; set; }
        public bool Milestone { get; set; }
        public string Version { get; set; }

    }

    #region enums
    public enum Status
        {
            NotSet,
            Planning,
            Started,
            Issue,
            ToBeReviewed,
            Done
        }

        public enum Priority
        {
            NotSet,
            Critical,
            High,
            Medium,
            Low
        }
        #endregion
    
}
