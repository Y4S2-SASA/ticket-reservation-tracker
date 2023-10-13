using MediatR;
using System.Linq.Expressions;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Application.Pipelines.Stations.Queries.GetStatusByIdQuery;
using TRT.Application.Pipelines.Trains.Queries.GetTrainById;
using TRT.Application.Pipelines.Users.Queries.GetUserById;
using TRT.Domain.Entities;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;
/*
 * File: GetReservationsByFilterQuery.cs
 * Purpose: Handle Get ReservationsBy Filter 
 * Author: Bartholomeusz S.V /IT20274702
*/
namespace TRT.Application.Pipelines.Reservations.Queries.GetReservationsByFilter
{
    public record GetReservationsByFilterQuery : IRequest<PaginatedListDTO<ReservationDetailDTO>>
    {
        public string? ReservationNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TrainId { get; set; }
        public string? DestinationStationId { get; set; }
        public string? ArrivalStationId { get; set; }
        public Status  Status { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class GetReservationsByFilterQueryHandler
                    : IRequestHandler<GetReservationsByFilterQuery, PaginatedListDTO<ReservationDetailDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IReservationQueryRepository _reservationQueryRepository;

        public GetReservationsByFilterQueryHandler
        (
            IMediator mediator,
            IReservationQueryRepository reservationQueryRepository
        )
        {
            this._mediator = mediator;
            this._reservationQueryRepository = reservationQueryRepository;
        }

        /// <summary>
        /// Handle  Get ReservationsBy Filter 
        /// </summary>
        /// <param name="request">>Contains Reservation filter paramaters </param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>paginated reservation details</returns>
        public async Task<PaginatedListDTO<ReservationDetailDTO>> Handle(GetReservationsByFilterQuery request, CancellationToken cancellationToken)
        {
            var totalRecordCount = NumberConstant.ZERO;
            var reservations = new List<ReservationDetailDTO>();

            Expression<Func<Reservation, bool>> query = x => true;

            query = ConfigureReservationFilter(query, request);

            totalRecordCount = (int)await _reservationQueryRepository.CountDocumentsAsync(query);

            var availableReservationData = await _reservationQueryRepository.GetPaginatedDataAsync
                                          (   query,
                                              request.PageSize,
                                              request.CurrentPage,
                                              cancellationToken
                                          );

            foreach(var reservation in availableReservationData)
            {
                var reservationData = new ReservationDetailDTO();

                var train = await _mediator.Send(new GetTrainByIdQuery(reservation.TrainId));
                var destinationStation = await _mediator.Send(new GetStationByIdQuery(reservation.DestinationStationId));
                var arrivalStation = await _mediator.Send(new GetStationByIdQuery(reservation.ArrivalStationId));
                var createdUser = await _mediator.Send(new GetUserByIdQuery(reservation.CreatedUserNIC));

                reservationData.Id = reservation.Id;
                reservationData.ReferenceNumber = reservation.ReferenceNumber;
                reservationData.PassengerClass = EnumHelper.GetEnumDescription(reservation.PassengerClass);
                reservationData.DestinationStationName = destinationStation.Name;
                reservationData.TrainName = train.TrainName;
                reservationData.ArrivalStationName = arrivalStation.Name;
                reservationData.DateTime = reservation.DateTime.ToString(DateTimeFormatConstant.DATE_WITH_TIME_FORMAT);
                reservationData.NoOfPassengers = reservation.NoOfPassengers;
                reservationData.Price = reservation.Price;
                reservationData.CreatedByUser = $"{createdUser.FirstName} {createdUser.LastName}";
                reservationData.Status = EnumHelper.GetEnumDescription(reservation.Status);

                reservations.Add(reservationData);
            }

            return new PaginatedListDTO<ReservationDetailDTO>
                     (
                         reservations,
                         totalRecordCount,
                         request.CurrentPage + ApplicationLevelConstant.PAGINATION_PAGE_INCREMENT,
                         request.PageSize
                     );
        }

        //Set Reservation Filter conditions
        private Expression<Func<Reservation, bool>> ConfigureReservationFilter(Expression<Func<Reservation, bool>> query, GetReservationsByFilterQuery request)
        {
            

            if(!string.IsNullOrEmpty(request.ReservationNumber))
            {
                query = x => x.ReferenceNumber == request.ReservationNumber;
            }

            if(request.FromDate != null && request.ToDate != null)
            {
                query = x => x.DateTime >= request.FromDate && x.DateTime <= request.ToDate.Value.AddDays(NumberConstant.ONE)
                                  .AddSeconds(NumberConstant.MINUSONE); ;
            }

            if(!string.IsNullOrEmpty(request.TrainId))
            {
                query = x => x.TrainId == request.TrainId;
            }

            if(!string.IsNullOrEmpty(request.DestinationStationId))
            {
                query = x => x.DestinationStationId == request.DestinationStationId;
            }

            if (!string.IsNullOrEmpty(request.ArrivalStationId))
            {
                query = x => x.DestinationStationId == request.DestinationStationId;
            }

            if(request.Status > NumberConstant.ZERO)
            {
                query = x => x.Status == request.Status;
            }

            return query;


        }
    }
}
