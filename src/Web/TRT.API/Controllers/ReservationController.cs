using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Application.Pipelines.Reservations.Commands.ChangeReservationStatus;
using TRT.Application.Pipelines.Reservations.Commands.DeleteReservation;
using TRT.Application.Pipelines.Reservations.Commands.SaveReservation;
using TRT.Application.Pipelines.Reservations.Queries.GetReservationById;
using TRT.Application.Pipelines.Reservations.Queries.GetReservationsByFilter;
using TRT.Application.Pipelines.Reservations.Queries.GetTraverlerReservation;
using TRT.Domain.Constants;
/*
 * File: ReservationController.cs
 * Purpose: Controller for managing the Reservations.
 * Author: Bartholomeusz S.V /IT20274702
*/
namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mediator">The mediator.</param>
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
        [HttpGet("getTraverlerReservation/{status}")]
        public async Task<IActionResult> GetTraverlerReservation(int status)
        {
            try
            {
                var response = await _mediator.Send(new GetTraverlerReservationQuery(status));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [Authorize(Roles = AuthorizedRoles.TravelAgent)]
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

        [HttpGet("getReservationById/{id}")]
        public async Task<IActionResult> GetReservationById(string id)
        {
            try
            {
                var response = await _mediator.Send(new GetReservationByIdQuery(id));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpDelete("deleteReservation/{id}")]
        public async Task<IActionResult> DeleteReservation(string id)
        {
            try
            {
                var response = await _mediator.Send(new DeleteReservationCommand(id));

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
