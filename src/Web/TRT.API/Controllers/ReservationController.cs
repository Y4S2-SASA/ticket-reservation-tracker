using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Application.Pipelines.Reservations.Commands.ChangeReservationStatus;
using TRT.Application.Pipelines.Reservations.Commands.SaveReservation;
using TRT.Application.Pipelines.Reservations.Queries.GetReservationsByFilter;
using TRT.Application.Pipelines.Reservations.Queries.GetTraverlerReservation;
using TRT.Domain.Constants;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IMediator _mediator;

        public ReservationController(ILogger<ReservationController> logger, IMediator mediator)
        {
            this._logger = logger;
            this._mediator = mediator;
        }

        [Authorize(Roles = AuthorizedRoles.TravelAgentAndTraveler)]
        [HttpPost("saveReservation")]
        public async Task<IActionResult> SaveReservation([FromBody] ReservationDTO reservationDTO)
        {
            try
            {
                var response = await _mediator.Send(new SaveReservationCommand()
                {
                    Reservation = reservationDTO
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [Authorize(Roles = AuthorizedRoles.Traveler)]
        [HttpGet("getTraverlerReservation")]
        public async Task<IActionResult> GetTraverlerReservation()
        {
            try
            {
                var response = await _mediator.Send(new GetTraverlerReservationQuery());

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [Authorize(Roles = AuthorizedRoles.BackOffice)]
        [HttpPost("getReservationsByFilter")]
        public async Task<IActionResult> GetReservationsByFilter(GetReservationsByFilterQuery reservationsByFilterQuery)
        {
            try
            {
                var response = await _mediator.Send(reservationsByFilterQuery);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPut("changeReservationStatus")]
        public async Task<IActionResult> ChangeReservationStatus(ChangeReservationStatusCommand changeReservationStatusCommand)
        {
            try
            {
                var response = await _mediator.Send(changeReservationStatusCommand);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
