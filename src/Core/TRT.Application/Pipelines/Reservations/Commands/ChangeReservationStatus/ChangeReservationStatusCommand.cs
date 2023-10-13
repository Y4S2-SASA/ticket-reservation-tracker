using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;
/*
 * File: ChangeReservationStatusCommand.cs
 * Purpose: Change Reservation Status
 * Author: Bartholomeusz S.V /IT20274702
*/
namespace TRT.Application.Pipelines.Reservations.Commands.ChangeReservationStatus
{
    public record ChangeReservationStatusCommand : IRequest<ResultDTO>
    {
        public string Id { get; set; }
        public Status Status { get; set; }
    }

    public class ChangeReservationStatusCommandHandler : IRequestHandler<ChangeReservationStatusCommand, ResultDTO>
    {
        private readonly IReservationQueryRepository _reservationQueryRepository;
        private readonly IReservationCommandRepository _reservationCommandRepository;
       
        public ChangeReservationStatusCommandHandler
        (
            IReservationQueryRepository reservationQueryRepository,
            IReservationCommandRepository reservationCommandRepositor
                 
        )
        {
            this._reservationQueryRepository = reservationQueryRepository;
            this._reservationCommandRepository = reservationCommandRepositor;
            
        }

        /// <summary>
        /// Handle Change Reservation Status
        /// </summary>
        /// <param name="request">>Contains Reservation id</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>ResultDTO</returns>
        public async Task<ResultDTO> Handle(ChangeReservationStatusCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationQueryRepository.GetById(request.Id, cancellationToken);

            if(reservation != null)
            {
                reservation.Status = request.Status;

                await _reservationCommandRepository.UpdateAsync(reservation, cancellationToken);

                return ResultDTO.Success(ResponseMessageConstant.RESERVATION_STATUS_CHANGE_SUCCESS_RESPONSE_MESSAGE);

            }
            else
            {
                return ResultDTO.Failure(new List<string>()
                {
                    ResponseMessageConstant.RESERVATION_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE
                }); ;
            }


        }
    }
}
