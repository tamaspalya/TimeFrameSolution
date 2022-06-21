using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFrameDesktopApp.DTOs;

namespace TimeFrameDesktopApp.ServiceLayer
{
    public interface ITimeFrameService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<bool> DeleteUserById(int id);
        IEnumerable<TeamDto> GetAllTeams();
        Task<bool> RegisterUserAsync(RegisterDto user);
    }
}
