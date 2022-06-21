using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeFrameLib.Models;
using TimeFrameLib.Repositories;
using System.Linq;
using System;

namespace TimeFrame.Tests.DataAccess
{
    public class UserRepositoryTests
    {
        public IUserRepository _userRepository;

        private User _user;
        private string _password;

        [SetUp]
        public async Task Setup()
        {
            _userRepository = new UserRepository(Configuration.CONNECTION_STRING);
            _user = new(){ Email="test@email.com", FirstName="test_first_name", LastName="test_last_name" };
            _password = "testpassword";
            _user.Id = await _userRepository.CreateAsync(_user, _password);
        }

        [TearDown]
        public async Task CleanUp()
        {
            await _userRepository.DeleteAsync(_user.Id);
        }

        [Test]
        public async Task GetByEmailAsync()
        {
            User user = await _userRepository.GetByEmailAsync(_user.Email);
            Assert.True(user.Id == _user.Id);
        }

        [Test]
        public async Task UpdatePasswordAsync()
        {
            bool res = await _userRepository.UpdatePasswordAsync(_user.Email, _password, "newpassword");
            Assert.True(res);
        }

        [Test]
        public async Task UpdateUser()
        {
            bool res = await _userRepository.UpdateAsync(_user.Id, new() { Email = "newTestEmail@email.com", FirstName = "new_first_name", LastName = "new_last_name" });
            Assert.True(res); 
        }

    }

}
