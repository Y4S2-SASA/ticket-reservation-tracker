using MediatR;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Schedules.Queries.GetSchedulePrice
{
    public record GetSchedulePriceQuery : IRequest<decimal>
    {
        public string SelectedTrainId { get; set; }
        public string DepartureStationId { get; set; }
        public string ArrivalStationId { get; set; }
        public string SelectedScheduleId { get; set; }
        public int PassengerCount { get; set; }
        public PassengerClass PassengerClass { get; set; }
       
    }

    public class GetSchedulePriceQueryHandler : IRequestHandler<GetSchedulePriceQuery, decimal>
    {
        private readonly IScheduleQueryRepository _scheduleQueryRepository;
        private readonly IMediator _mediator;
        public GetSchedulePriceQueryHandler
        (
            IScheduleQueryRepository scheduleQueryRepository,
            IMediator mediator
        )
        {
            this._scheduleQueryRepository = scheduleQueryRepository;
            this._mediator = mediator;
        }
        public async Task<decimal> Handle(GetSchedulePriceQuery request, CancellationToken cancellationToken)
        {
            var schedule = await _scheduleQueryRepository.GetById(request.SelectedScheduleId, cancellationToken);

            var subStations = schedule.SubStationDetails.ToList();

            var startIndex = subStations.FindIndex(station => station.StationId == request.ArrivalStationId);
            var endIndex = subStations.FindIndex(station => station.StationId == request.DepartureStationId);


            
        }
    }
}
