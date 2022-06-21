using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TimeFrameWebAPIClient.DTOs;
using TimeFrameLib.Models;
using TimeFrameWebAPIClient;

namespace TimeFrame.Tests.WebApiClient
{
    internal class UserWebApiTests
    {

        ITimeFrameApiClient _client;


        UserDto _user;
        string _password;

        [SetUp]
        public async Task SetUp()
        {
            _client = new TimeFrameApiClient(Configuration.LOCAL_URI);
            _user = new UserDto() { FirstName = "Test", LastName = "User", Email = "testuser@tests.com" };
            _password = "usertest1";
            RegisterDto registration = new RegisterDto() { Email = _user.Email, FirstName = _user.FirstName, LastName = _user.LastName, Password = _password };
            _user.Id = await _client.CreateUserAsync(registration);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _client.DeleteUserAsync(_user.Id);

        }

        [Test]
        public async Task GetAllUsersAsync() {
            //arrange
            //act
            IEnumerable<UserDto> users = await _client.GetAllUsersAsync();
            //assert
            Assert.IsNotNull(users);
        }
        [Test]
        public async Task GetUserByEmail()
        {
            //arrange
            string email = _user.Email;
            //act
            UserDto user = await _client.GetUserByEmailAsync(email);
            //assert
            Assert.IsNotNull(user);

        }
        [Test]
        public async Task GetUserByIdAsync()
        {
            //arrange
            int id = _user.Id;
            //act
            UserDto user = await _client.GetUserByIdAsync(id);
            //assert
            Assert.IsNotNull(user);

        }
        [Test]
        public async Task LoginAsync()
        {
            //arrange
            LoginDto login = new LoginDto() { Email = _user.Email, Password = _password };
            //act
            int id = await _client.LoginAsync(login);
            //assert
            Assert.IsTrue(id == _user.Id);
        }
        [Test]
        public async Task UpdateUserAsync(){
            //arrange
            UserDto newuser = new UserDto() {Id=_user.Id, Email="newemail@ucn.dk", FirstName= "New", LastName="User2"};
            //act
            bool successful = await _client.UpdateUserAsync(newuser);
            //assert
            Assert.IsTrue(successful);

        }
        /*
        [Test]
        public async Task UpdateUserPasswordAsync()
        {
            //arrange
            int userid = _user.Id;
            string newpassword = "newpassword";
            //act
            bool successful = await _client.UpdateUserPasswordAsync(userid, _password, newpassword);
            //assert
            Assert.IsTrue(successful);
        }
        */
        //feature isnt implemented, commenting out so only tests that are implemented run
    }

}
/*
 *  Task<int> CreateUserAsync(RegisterDto entity);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<int> LoginAsync(LoginDto user);
        Task<bool> UpdateUserAsync(UserDto entity);
        Task<bool> UpdateUserPasswordAsync(UserDto user);
*/