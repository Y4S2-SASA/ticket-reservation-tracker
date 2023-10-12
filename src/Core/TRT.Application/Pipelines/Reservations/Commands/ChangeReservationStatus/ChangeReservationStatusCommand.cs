using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Reservations.Commands.ChangeReservationStatus
{
    public record ChangeReservationStatusCommand : IRequest<ResultDTO>
    {
        public StatusChangeDTO StatusChangeDTO { get; set; }
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
        public async Task<ResultDTO> Handle(ChangeReservationStatusCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationQueryRepository.GetById(request.StatusChangeDTO.Id, cancellationToken);

            if(reservation != null)
            {
                reservation.Status = request.StatusChangeDTO.Status;

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
