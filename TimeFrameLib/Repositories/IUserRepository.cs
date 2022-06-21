using TimeFrameLib.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeFrameLib.Repositories
{
    public interface IUserRepository
    {
        //Default CRUD methods for Repository (DAO pattern)
        Task<int> CreateAsync(User entity, string password);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, User entity);
        Task<bool> DeleteAsync(int id);

        //methods relating to handling security
        Task<bool> UpdatePasswordAsync(string email, string oldPassword, string newPassword);
        Task<int> LoginAsync(string email, string password);
        Task<User> GetByEmailAsync(string email);
    }
}