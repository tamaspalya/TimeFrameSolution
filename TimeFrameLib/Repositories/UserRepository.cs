using Dapper;
using TimeFrameLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeFrameLib.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {

    public UserRepository(string connectionstring) : base(connectionstring) { }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var query = "DELETE FROM [User] WHERE Id=@Id";
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(query, new { id }) > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting User with id {id}: '{ex.Message}'.", ex);
            }
        }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        try
        {
            var query = "SELECT * FROM [User]";
            using var connection = CreateConnection();
            return (await connection.QueryAsync<User>(query)).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting all Users: '{ex.Message}'.", ex);
        }
    }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                var query = "SELECT * FROM [User] WHERE Id=@Id";
                using var connection = CreateConnection();
                return await connection.QuerySingleAsync<User>(query, new { id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting User with id {id}: '{ex.Message}'.", ex);
            }
        }


        public async Task<bool> UpdateAsync(int Id, User entity)
        {
            try
            {
                var query = "UPDATE [User] SET Email=@Email, FirstName=@FirstName, LastName=@LastName WHERE Id=@Id;";
                using var connection = CreateConnection();
                entity.Id = Id;
                return await connection.ExecuteAsync(query, entity) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating User: '{ex.Message}'.", ex);
            }
        }

        public async Task<int> CreateAsync(User entity, string password)
        {
            try
            {
                var query = "INSERT INTO [User] (Email, FirstName, LastName, PasswordHash) OUTPUT INSERTED.Id VALUES (@Email, @FirstName, @LastName, @PasswordHash);";
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
                using var connection = CreateConnection();
                return await connection.QuerySingleAsync<int>(query, new { Email = entity.Email, FirstName = entity.FirstName, LastName = entity.LastName, PasswordHash = passwordHash });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating new User: '{ex.Message}'.", ex);
            }

        }

        public async Task<int> LoginAsync(string email, string password)
        {
            try
            {
                var query = "SELECT Id, PasswordHash FROM [User] WHERE Email=@Email";
                using var connection = CreateConnection();

                var UserTuple = await connection.QuerySingleAsync<UserTuple>(query, new { Email = email });
                if (BCrypt.Net.BCrypt.Verify(password, UserTuple.PasswordHash))
                {
                    return UserTuple.Id;
                }
                return -1;
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging in for User with email {email}: '{ex.Message}'.", ex);
            }
        }

        public async Task<bool> UpdatePasswordAsync(string email, string oldPassword, string newPassword)
        {
            try
            {
                var query = "UPDATE [User] SET PasswordHash=@NewPasswordHash WHERE Id=@Id;";
                var id = await LoginAsync(email, oldPassword);
                if (id > 0)
                {
                    var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, BCrypt.Net.BCrypt.GenerateSalt(12));
                    using var connection = CreateConnection();
                    return await connection.ExecuteAsync(query, new { Id = id, NewPasswordHash = newPasswordHash }) > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating User: '{ex.Message}'.", ex);
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                var query = "SELECT * FROM [User] WHERE Email=@Email;";
                using var connection = CreateConnection();
                return await connection.QuerySingleAsync<User>(query, new { email }); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting an author with email: '{ex.Message}'.", ex);
            }
        }

        internal class UserTuple
        {
            public int Id;
            public string PasswordHash;
        }
    }
}
