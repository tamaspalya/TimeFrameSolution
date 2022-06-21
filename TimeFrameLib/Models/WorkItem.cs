using System;
using System.Collections.Generic;

namespace TimeFrameLib.Models
{
    public class WorkItem
    {
        public int ProjectId { get; set; }

        public int Id { get; set; }
        public string WorkItemName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public List<int> PeopleAssignedId { get; set; }
        public string Description { get; set; }
        public bool Milestone { get; set; }
        public byte[] Version { get; set; }


        public WorkItem() { }

        public WorkItem(string _WorkItemName, int _ProjectId)
        {
            WorkItemName = _WorkItemName;
            ProjectId = _ProjectId;
        }
    }

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
}
