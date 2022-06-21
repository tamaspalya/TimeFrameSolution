using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeFrameLib.Models
{
    public class Project
    {
        public int TeamID { get; set; } //keeping it team for now, will change it later, non-editable
        // public Template ProjectTemplate { get; } //same as team, non-editable
        public int ID { get; set; }
        public string ProjectName { get; set; } //name of the project, editable
        public string ProjectDescription { get; set; } //project description, editable
        public bool IsPrivate { get; set; } //private or public 

        public Project (int teamID)
        {
            TeamID = teamID;
            IsPrivate = true;
        }

        public Project()
        {
            IsPrivate = true;
        }

        //public bool SetTemplate(Template projectTemp)
        //{
        //    if (projectTemp != null)
        //    {
        //        ProjectTemplate = projectTemp;
        //        return true;
        //    }
        //    return false;
        //}

    }
}
