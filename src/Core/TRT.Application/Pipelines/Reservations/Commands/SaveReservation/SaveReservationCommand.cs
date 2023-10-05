using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Extentions;
using TRT.Application.Common.Helpers;
using TRT.Application.Common.Interfaces;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Entities;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Reservations.Commands.SaveReservation
{
    public record SaveReservationCommand : IRequest<ResultDTO>
    {
        public ReservationDTO Reservation { get; set; }
    }

    public class SaveReservationCommandHandler : IRequestHandler<SaveReservationCommand, ResultDTO>
    {
        private readonly IReservationQueryRepository _reservationQueryRepository;
        private readonly IReservationCommandRepository _reservationCommandRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SaveReservationCommandHandler> _logger;
        public SaveReservationCommandHandler
        (
            IReservationQueryRepository reservationQueryRepository,
            IReservationCommandRepository reservationCommandRepositor,
            ICurrentUserService currentUserService,
            ILogger<SaveReservationCommandHandler> logger
        )
        {
            this._reservationQueryRepository = reservationQueryRepository;
            this._reservationCommandRepository = reservationCommandRepositor;
            this._currentUserService = currentUserService;
            this._logger = logger;
        }
        public async Task<ResultDTO> Handle(SaveReservationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(string.IsNullOrEmpty(request.Reservation.Id))
                {
                    var reservation = request.Reservation.ToEntity();

                    reservation.ReferenceNumber = ReservationNumberGenerator.GenerateTicketReferenceCode();
                    reservation.Status = Status.Activated;
                    reservation.CreatedUserNIC = _currentUserService.UserId;

                    await _reservationCommandRepository.AddAsync(reservation, cancellationToken);

                }

                return ResultDTO.Success(ResponseMessageConstant.RESERVATION_SAVE_SUCCESS_RESPONSE_MESSAGE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                throw;
            }
        }
    }
}
