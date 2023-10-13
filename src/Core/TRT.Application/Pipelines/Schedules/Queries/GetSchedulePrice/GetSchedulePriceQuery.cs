using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.Pipelines.Trains.Queries.GetTrainById;
using TRT.Application.Pipelines.TrainTicketPrices.Queries.GetTrainTicketPriceByClass;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;

/*
 * File: GetSchedulePriceQuery.cs
 * Purpose: Handle GetSchedulePrice
 * Author: Perera M.S.D/IT20020262
*/
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

        /// <summary>
        /// Handle GetSchedulePrice.
        /// </summary>
        /// <param name="request">>Contains SchedulePrice parameters</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>price</returns>
        public async Task<decimal> Handle(GetSchedulePriceQuery request, CancellationToken cancellationToken)
        {
            var price = NumberConstant.DECIMAL_ZERO;
            var reservationPerPersonPrice = NumberConstant.DECIMAL_ZERO;
            var rangeCountSubstations = NumberConstant.ZERO;

            var schedule = await _scheduleQueryRepository.GetById(request.SelectedScheduleId, cancellationToken);

            var subStations = schedule.SubStationDetails.ToList();

            var startIndex = subStations.FindIndex(station => station.StationId == request.ArrivalStationId);

            var endIndex = subStations.FindIndex(station => station.StationId == request.DepartureStationId);

            if (startIndex != NumberConstant.MINUSONE && 
                    endIndex != NumberConstant.MINUSONE && startIndex < endIndex) 
            {
               rangeCountSubstations = endIndex - startIndex;

               var train = await _mediator
                                    .Send(new GetTrainByIdQuery(request.SelectedTrainId), cancellationToken);

                var ticketPrice = await _mediator.Send(new GetTrainTicketPriceByClass()
                {
                    PassengerClass = request.PassengerClass,
                }, cancellationToken);

                reservationPerPersonPrice = rangeCountSubstations * ticketPrice.Price;

                price = reservationPerPersonPrice * request.PassengerCount;

                return price;
            }
            else
            {
                return price;
            }

        }
    }
}
