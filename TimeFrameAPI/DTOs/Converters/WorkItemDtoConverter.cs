using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFrameLib.Models;

namespace TimeFrameAPI.DTOs.Converters
{
    public static class WorkItemDtoConverter
    {
        public static WorkItemDto ToDto(this WorkItem workItemToConvert)
        {
            var workItemDto = new WorkItemDto();
            workItemToConvert.CopyPropertiesTo(workItemDto);
            workItemDto.Status = (Status)workItemToConvert.Status;
            workItemDto.Priority = (Priority)workItemToConvert.Priority;
            workItemDto.Version = Convert.ToBase64String(workItemToConvert.Version);
            return workItemDto;
        }

        public static WorkItem FromDto(this WorkItemDto workItemDtoToConvert)
        {
            var workItem = new WorkItem();
            workItemDtoToConvert.CopyPropertiesTo(workItem);
            workItem.Status = (TimeFrameLib.Models.Status)workItemDtoToConvert.Status;
            workItem.Priority = (TimeFrameLib.Models.Priority)workItemDtoToConvert.Priority;
            workItem.Version = Convert.FromBase64String(workItemDtoToConvert.Version);

            return workItem;
        }

        public static IEnumerable<WorkItemDto> ToDtos(this IEnumerable<WorkItem> workItemsToConvert)
        {
            foreach (var workItem in workItemsToConvert)
            {
                yield return workItem.ToDto();
            }
        }

        public static IEnumerable<WorkItem> FromDtos(this IEnumerable<WorkItemDto> workItemDtosToConvert)
        {
            foreach (var workItemDto in workItemDtosToConvert)
            {
                yield return workItemDto.FromDto();
            }
        }
    }
}
