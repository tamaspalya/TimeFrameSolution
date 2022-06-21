using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameWebAPIClient.DTOs
{
    public class ProjectDto
    {
        public string ProjectName { get; set; }
        public int TeamID { get; set; }
        public int ID { get; set; }
        public string ProjectDescription { get; set; }
        public bool IsPrivate { get; set; }
    }
}
