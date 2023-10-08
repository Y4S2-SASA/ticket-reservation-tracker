using MediatR;
using TRT.Application.DTOs.Common;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Stations.Queries.GetAllStationMasterData
{
    public record GetAllStationMasterDataQuery : IRequest<List<DropDownCoreDTO>>
    {

    }

    public class GetAllStationMasterDataQueryHanlder : IRequestHandler<GetAllStationMasterDataQuery, List<DropDownCoreDTO>>
    {
        private readonly IStationQueryRepository _stationQueryRepository;
        public GetAllStationMasterDataQueryHanlder(IStationQueryRepository stationQueryRepository)
        {
            this._stationQueryRepository = stationQueryRepository;
        }
        public async Task<List<DropDownCoreDTO>> Handle(GetAllStationMasterDataQuery request, CancellationToken cancellationToken)
        {
            var stations = await _stationQueryRepository.GetAll(cancellationToken);

            return stations.Select(x=> new DropDownCoreDTO()
            {
                Id = x.Id,
                Name = x.Name,

            }).ToList();
        }
    }
}
