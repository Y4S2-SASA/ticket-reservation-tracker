﻿using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.Common.Interfaces;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Application.Pipelines.Stations.Queries.GetStatusByIdQuery;
using TRT.Application.Pipelines.Trains.Queries.GetTrainById;
using TRT.Application.Pipelines.Users.Queries.GetUserById;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;
/*
 * File: GetTraverlerReservationQuery.cs
 * Purpose: Handle Get Traverler Reservation
 * Author: Bartholomeusz S.V /IT20274702
*/
namespace TRT.Application.Pipelines.Reservations.Queries.GetTraverlerReservation
{
    public record GetTraverlerReservationQuery(int Status) : IRequest<List<ReservationDetailDTO>>
    {
    }

    public class GetTraverlerReservationQueryHandler : IRequestHandler<GetTraverlerReservationQuery, List<ReservationDetailDTO>>
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IReservationQueryRepository _reservationQueryRepository;

        public GetTraverlerReservationQueryHandler
        (
            IMediator mediator,
            ICurrentUserService currentUserService,
            IReservationQueryRepository reservationQueryRepository
        )
        {
            this._mediator = mediator;
            this._currentUserService = currentUserService;
            this._reservationQueryRepository = reservationQueryRepository;
        }

        /// <summary>
        /// Handle  Get Traverler Reservation
        /// </summary>
        /// <param name="request">>Contains status parameter</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>list of reservation details</returns>
        public async Task<List<ReservationDetailDTO>> Handle(GetTraverlerReservationQuery request, CancellationToken cancellationToken)
        {
            var reservations = new List<ReservationDetailDTO>();

            var listOfReservation = (await _reservationQueryRepository
                                        .Query(x => x.CreatedUserNIC == _currentUserService.UserId && 
                                        x.Status == (Status)request.Status))
                                        .ToList();

            foreach(var reservation in listOfReservation)
            {
                var reservationData = new ReservationDetailDTO();

                var train = await _mediator.Send(new GetTrainByIdQuery(reservation.TrainId));
                var destinationStation = await _mediator.Send(new GetStationByIdQuery(reservation.DestinationStationId));
                var arrivalStation = await _mediator.Send(new GetStationByIdQuery(reservation.ArrivalStationId));
                var createdUser = await _mediator.Send(new GetUserByIdQuery(_currentUserService.UserId));

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

            return reservations;
        }
    }
}
