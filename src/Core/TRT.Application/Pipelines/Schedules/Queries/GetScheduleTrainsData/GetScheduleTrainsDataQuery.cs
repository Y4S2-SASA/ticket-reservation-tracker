using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Interfaces;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Application.Pipelines.Trains.Queries.GetTrainById;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Schedules.Queries.GetScheduleTrainsData
{
    public record GetScheduleTrainsDataQuery : IRequest<List<ReservationTrainDetailDTO>>
    {
        public string DestinationStationId { get; set; }
        public string StartPointStationId { get; set; }
        public DateTime DateTime { get; set; }
        public PassengerClass PassengerClass { get; set; }
    }

    public class GetScheduleTrainsDataQueryHandler
                    : IRequestHandler<GetScheduleTrainsDataQuery, List<ReservationTrainDetailDTO>>
    {
        private readonly IScheduleQueryRepository _scheduleQueryRepository;
        private readonly IMediator _mediator;
       
        public GetScheduleTrainsDataQueryHandler
        (
            IScheduleQueryRepository scheduleQueryRepository,
            IMediator mediator
        )
        {
            this._scheduleQueryRepository = scheduleQueryRepository;
            this._mediator = mediator;
        }
        public async Task<List<ReservationTrainDetailDTO>> Handle(GetScheduleTrainsDataQuery request, CancellationToken cancellationToken)
        {
            var reservationTrainDetails = new List<ReservationTrainDetailDTO>();

            var startDate = DateTime.Parse(request.DateTime.ToString());

            var endDate = DateTime.Parse(request.DateTime.ToString())
                                  .AddDays(NumberConstant.ONE)
                                  .AddSeconds(NumberConstant.MINUSONE);

            var listOfSchedules = (await _scheduleQueryRepository
                                 .Query(x => x.SubStationDetails.Any(s => s.StationId == request.DestinationStationId && s.StationId == request.StartPointStationId) &&
                                 x.DepartureTime >= startDate && x.DepartureTime <= endDate))
                                 .ToList();

            foreach (var schedule in listOfSchedules)
            {
                var startPointStation = schedule.SubStationDetails
                                        .FirstOrDefault(x => x.StationId == request.StartPointStationId);

                var trainDetail = await _mediator.Send(new GetTrainByIdQuery(schedule.TrainId));

                reservationTrainDetails.Add(new ReservationTrainDetailDTO()
                {
                    TrainId = trainDetail.Id,
                    TrainName = trainDetail.TrainName,
                    ArrivalTime = startPointStation.ArrivalTime
                });
            }

            return reservationTrainDetails;
        }
    }
}
