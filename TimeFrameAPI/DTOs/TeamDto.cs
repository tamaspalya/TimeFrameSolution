using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeFrameAPI.DTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<UserDto> TeamMembers { get; set; }
    }
}
