using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameAPI.DTOs.Converters
{
    public static class ProjectDtoConverter
    {
        public static ProjectDto ToDto(this Project projectToConvert)
        {
            var projectDto = new ProjectDto();
            projectToConvert.CopyPropertiesTo(projectDto);
            return projectDto;
        }

        public static Project FromDto(this ProjectDto projectDtoToConvert)
        {
            var project = new Project();
            projectDtoToConvert.CopyPropertiesTo(project);
            return project;
        }

        public static IEnumerable<ProjectDto> ToDtos(this IEnumerable<Project> projectsToConvert)
        {
            foreach (var project in projectsToConvert)
            {
                yield return project.ToDto();
            }
        }

        public static IEnumerable<Project> FromDtos(this IEnumerable<ProjectDto> projectDtosToConvert)
        {
            foreach (var projectDto in projectDtosToConvert)
            {
                yield return projectDto.FromDto();
            }
        }
    }
}
