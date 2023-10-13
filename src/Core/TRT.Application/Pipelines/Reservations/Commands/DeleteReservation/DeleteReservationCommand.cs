using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;
/*
 * File: DeleteReservationCommand.cs
 * Purpose: Delete reservation
 * Author: Bartholomeusz S.V /IT20274702
*/
namespace TRT.Application.Pipelines.Reservations.Commands.DeleteReservation
{
    public record DeleteReservationCommand(string id) : IRequest<ResultDTO>
    {
    }

    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, ResultDTO>
    {
        private readonly IReservationQueryRepository _reservationQueryRepository;
        private readonly IReservationCommandRepository _reservationCommandRepository;

        public DeleteReservationCommandHandler(IReservationQueryRepository reservationQueryRepository, IReservationCommandRepository reservationCommandRepository)
        {
            this._reservationQueryRepository = reservationQueryRepository;
            this._reservationCommandRepository = reservationCommandRepository;
        }

        /// <summary>
        /// Handle Delete reservation
        /// </summary>
        /// <param name="request">>Contains Reservation Id</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>ResultDTO</returns>
        public async Task<ResultDTO> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationQueryRepository.GetById(request.id, cancellationToken);

            if(reservation == null)
            {
                return ResultDTO.Failure(new List<string>()
                {
                    ResponseMessageConstant.RESERVATION_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE
                });
            }
            else
            {
                await _reservationCommandRepository.DeleteAsync(reservation, cancellationToken);

                return ResultDTO.Success(ResponseMessageConstant.RESERVATION_DELETE_SUCCESS_RESPONSE_MESSAGE);
            }
           
        }
    }
}
