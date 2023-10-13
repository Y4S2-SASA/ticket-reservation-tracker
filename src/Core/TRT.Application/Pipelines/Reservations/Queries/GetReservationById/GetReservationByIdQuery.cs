using MediatR;
using TRT.Application.Common.Extentions;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Domain.Repositories.Query;
/*
 * File: GetReservationByIdQuery.cs
 * Purpose: Get Reservation ById
 * Author: Bartholomeusz S.V /IT20274702
*/
namespace TRT.Application.Pipelines.Reservations.Queries.GetReservationById
{
    public record GetReservationByIdQuery(string id) : IRequest<ReservationDTO>;


    public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, ReservationDTO>
    {
        private readonly IReservationQueryRepository _reservationQueryRepository;
        public GetReservationByIdQueryHandler(IReservationQueryRepository reservationQueryRepository)
        {     
            this._reservationQueryRepository = reservationQueryRepository;
        }

        /// <summary>
        /// Handle Get Reservation ById
        /// </summary>
        /// <param name="request">>Contains Reservation id</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>Reservation Details</returns>
        public async Task<ReservationDTO> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationQueryRepository.GetById(request.id, cancellationToken);

           return reservation.ToDto();

        }
    }
}
