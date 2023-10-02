using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TRT.Application.DTOs.Common;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Stations.Queries.GetStationsMasterData
{
    public record GetStationsMasterDataQuery : IRequest<List<DropDownCoreDTO>>
    {
        public string? SearchText { get; set; }
    }

    public class GetStationsMasterDataQueryHandler : IRequestHandler<GetStationsMasterDataQuery, List<DropDownCoreDTO>>
    {
        private readonly IStationQueryRepository _stationQueryRepository;
        private readonly ILogger<GetStationsMasterDataQueryHandler> _logger;

        public GetStationsMasterDataQueryHandler
        (
            IStationQueryRepository stationQueryRepository,
            ILogger<GetStationsMasterDataQueryHandler> logger
        )
        {
            this._stationQueryRepository = stationQueryRepository;
            this._logger = logger;
        }
        public async Task<List<DropDownCoreDTO>> Handle(GetStationsMasterDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<Station, bool>> query = x => true;

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    query = x => x.Name.ToLower().Contains(request.SearchText.ToLower());
                }

                var listOfStations = (await _stationQueryRepository.Query(query))
                                    .ToList();

                return listOfStations.Select(x=> new DropDownCoreDTO()
                {
                    Id = x.Id,
                    Name = x.Name,

                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
