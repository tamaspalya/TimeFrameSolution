using System;
using System.Collections.Generic;
using TimeFrameLib.Models;

namespace TimeFrameAPI.DTOs.Converters
{
    public static class TeamDtoConverter
    {
        public static TeamDto ToDto(this Team teamToConvert)
        {
            var teamDto = new TeamDto();
            List<UserDto> MembersList = new List<UserDto>();
            foreach (var users in teamToConvert.TeamMembers)
            {
                MembersList.Add(UserDtoConverter.ToDto(users));
            }
            teamToConvert.CopyPropertiesTo(teamDto);
            teamDto.TeamMembers = MembersList;
            return teamDto;
        }

        public static Team FromDto(this TeamDto teamDtoToConvert)
        {
            var team = new Team();
            List<User> MembersList = new List<User>();
            foreach (var users in teamDtoToConvert.TeamMembers) MembersList.Add(UserDtoConverter.FromDto(users));
            teamDtoToConvert.CopyPropertiesTo(team);
            team.TeamMembers = MembersList;
            return team;
        }

        public static IEnumerable<TeamDto> ToDtos(this IEnumerable<Team> teamsToConvert)
        {
            foreach (var team in teamsToConvert)
            {
                yield return team.ToDto();
            }
        }

        public static IEnumerable<Team> FromDtos(this IEnumerable<TeamDto> teamDtosToConvert)
        {
            foreach (var teamDto in teamDtosToConvert)
            {
                yield return teamDto.FromDto();
            }
        }
    }
}
