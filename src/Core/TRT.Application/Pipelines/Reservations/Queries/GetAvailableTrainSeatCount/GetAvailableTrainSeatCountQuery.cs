using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.Pipelines.Trains.Queries.GetTrainById;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Reservations.Queries.GetAvailableTrainSeatCount
{
    public record GetAvailableTrainSeatCountQuery : IRequest<int>
    {
        public string TrainId { get; set; }
        public string DestinationStationId { get; set; }
        public string ArrivalStationId { get; set; }
        public DateTime ReservationDate { get; set; }
        
    }

    public class GetAvailableTrainSeatCountQueryHandler : IRequestHandler<GetAvailableTrainSeatCountQuery, int>
    {
        private readonly IReservationQueryRepository _reservationQueryRepository;
        private readonly IMediator _mediator;
        public GetAvailableTrainSeatCountQueryHandler
        (
            IReservationQueryRepository reservationQueryRepository,
            IMediator mediator
        )
        {
            this._reservationQueryRepository = reservationQueryRepository;
            this._mediator = mediator;
        }
        public async Task<int> Handle(GetAvailableTrainSeatCountQuery request, CancellationToken cancellationToken)
        {
            var reservedSeatCount = NumberConstant.ZERO;

            var startDate = DateTime.Parse(request.ReservationDate.ToString());

            var endDate = DateTime.Parse(request.ReservationDate.ToString())
                                  .AddDays(NumberConstant.ONE)
                                  .AddSeconds(NumberConstant.MINUSONE);

            var listOfReservationList = (await _reservationQueryRepository.Query(x=>x.TrainId == request.TrainId &&
                                        x.DestinationStationId == request.DestinationStationId && 
                                        x.ArrivalStationId == request.ArrivalStationId && 
                                        x.DateTime >= startDate && x.DateTime <= endDate))
                                        .ToList();

            reservedSeatCount = listOfReservationList.Sum(item => item.NoOfPassengers);

            var train = await _mediator
                                    .Send(new GetTrainByIdQuery(request.TrainId));

            if (train == null || train.SeatCapacity == reservedSeatCount)
            {
                return NumberConstant.ZERO;
            }

            return train.SeatCapacity - reservedSeatCount;

        }
    }
}
