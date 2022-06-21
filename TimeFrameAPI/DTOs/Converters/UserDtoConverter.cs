using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameAPI.DTOs.Converters
{
    public static class UserDtoConverter
    {
        public static UserDto ToDto(this User userToConvert)
        {
            var userDto = new UserDto();
            userToConvert.CopyPropertiesTo(userDto);
            return userDto;
        }

        public static User FromDto(this UserDto userDtoToConvert)
        {
            var user = new User();
            userDtoToConvert.CopyPropertiesTo(user);
            return user;
        }

        public static IEnumerable<UserDto> ToDtos(this IEnumerable<User> usersToConvert)
        {
            foreach (var user in usersToConvert)
            {
                yield return user.ToDto();
            }
        }

        public static IEnumerable<User> FromDtos(this IEnumerable<UserDto> userDtosToConvert)
        {
            foreach (var userDto in userDtosToConvert)
            {
                yield return userDto.FromDto();
            }
        }
    }
}

