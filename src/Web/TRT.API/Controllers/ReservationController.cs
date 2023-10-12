using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Application.Pipelines.Reservations.Commands.SaveReservation;
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
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize(Roles = AuthorizedRoles.Traveler)]
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
    }
}
