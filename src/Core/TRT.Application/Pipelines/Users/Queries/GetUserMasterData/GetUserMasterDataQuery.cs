using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Enums;

namespace TRT.Application.Pipelines.Users.Queries.GetUserMasterData
{
    public record GetUserMasterDataQuery : IRequest<UserMasterDataDTO>
    {
    }

    public class GetUserMasterDataQueryHandler : IRequestHandler<GetUserMasterDataQuery, UserMasterDataDTO>
    {
        public async Task<UserMasterDataDTO> Handle(GetUserMasterDataQuery request, CancellationToken cancellationToken)
        {
            var masterData = new UserMasterDataDTO();


            masterData.Roles =  Enum.GetValues(typeof(Role))
                           .Cast<Role>()
                           .Select(x => new DropDownDTO
                           {
                               Id = (int)x,
                               Name = EnumHelper.GetEnumDescription(x),
                           })
                           .ToList();
          

            masterData.Status = Enum.GetValues(typeof(Status))
                           .Cast<Status>()
                           .Select(x => new DropDownDTO
                           {
                               Id = (int)x,
                               Name = EnumHelper.GetEnumDescription(x),
                           })
                           .ToList();

            var defaultValue = new DropDownDTO()
            {
                Id = NumberConstant.ZERO,
                Name = "-All-"
            };

            masterData.Roles.Insert(NumberConstant.ZERO, defaultValue); 
            masterData.Status.Insert(NumberConstant.ZERO, defaultValue); 


            return masterData;
        }
    }
}
