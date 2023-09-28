using MediatR;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.Common;
using TRT.Domain.Enums;

namespace TRT.Application.Pipelines.Users.Queries.GetUserMasterData
{
    public record GetUserMasterDataQuery : IRequest<List<DropDownDTO>>
    {
    }

    public class GetUserMasterDataQueryHandler : IRequestHandler<GetUserMasterDataQuery, List<DropDownDTO>>
    {
       
        public async Task<List<DropDownDTO>> Handle(GetUserMasterDataQuery request, CancellationToken cancellationToken)
        {
            var roles = await Task.Run(() =>
            {
                return Enum.GetValues(typeof(Role))
                           .Cast<Role>()
                           .Select(x => new DropDownDTO
                           {
                               Id = (int)x,
                               Name = EnumHelper.GetEnumDescription(x),
                           })
                           .ToList();
            });

            return roles;
        }
    }
}
