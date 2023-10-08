using MediatR;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Stations.Queries.GetStatusByIdQuery
{
    public record GetStationByIdQuery(string id) : IRequest<Station>
    {
    }

    public class GetStationByIdQueryHandler : IRequestHandler<GetStationByIdQuery, Station>
    {
        private readonly IStationQueryRepository _stationQueryRepository;

        public GetStationByIdQueryHandler(IStationQueryRepository stationQueryRepository)
        {
            this._stationQueryRepository = stationQueryRepository;
        }
        public async Task<Station> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
        {
            var station = await _stationQueryRepository.GetById(request.id, cancellationToken);

            return station;
        }
    }
}
