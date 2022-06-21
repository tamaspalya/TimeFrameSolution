using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeFrameLib.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<User> TeamMembers { get; set; }
    }
}
