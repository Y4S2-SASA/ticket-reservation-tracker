using MediatR;
using System.Linq.Expressions;
using TRT.Application.DTOs.DashboardDTOs;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Dashboards.Queries.GetDashboardMasterData
{
    public record GetDashboardMasterDataQuery : IRequest<DashboardMasterDataDTO>
    {

    }

    public class GetDashboardMasterDataQueryHandler : IRequestHandler<GetDashboardMasterDataQuery, DashboardMasterDataDTO>
    {
        public readonly IUserQueryRepository _userQueryRepository;
        public readonly IStationQueryRepository _stationQueryRepository;
        public readonly IReservationQueryRepository _reservationQueryRepository;
        public readonly ITrainQueryRepository _trainQueryRepository;
        public GetDashboardMasterDataQueryHandler
        (
            IUserQueryRepository userQueryRepository, 
            IStationQueryRepository stationQueryRepository,
            IReservationQueryRepository reservationQueryRepository, 
            ITrainQueryRepository trainQueryRepository
        )
        {
            this._userQueryRepository = userQueryRepository;
            this._stationQueryRepository = stationQueryRepository;
            this._reservationQueryRepository = reservationQueryRepository;
            this._trainQueryRepository = trainQueryRepository;

        }
        public async Task<DashboardMasterDataDTO> Handle(GetDashboardMasterDataQuery request, CancellationToken cancellationToken)
        {
            var dashboardData = new DashboardMasterDataDTO();

            Expression<Func<User, bool>> userQuery = x => x.Status == Domain.Enums.Status.Activated;
            Expression<Func<Station, bool>> stationQuery = x => true;
            Expression<Func<Reservation, bool>> reservationQuery = x => x.Status == Domain.Enums.Status.Activated;
            Expression<Func<Train, bool>> trainQuery = x => x.Status == Domain.Enums.Status.Activated;


            dashboardData.ActiveUserCount = (int) await _userQueryRepository.CountDocumentsAsync(userQuery);
            dashboardData.StationCount = (int)await _stationQueryRepository.CountDocumentsAsync(stationQuery);
            dashboardData.ActiveReservationCount = (int)await _reservationQueryRepository.CountDocumentsAsync(reservationQuery);
            dashboardData.ActiveTrainCount = (int)await _trainQueryRepository.CountDocumentsAsync(trainQuery);

            return dashboardData;
        }
    }
}
