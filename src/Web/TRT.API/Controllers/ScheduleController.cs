using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.ScheduleDTOs;
using TRT.Application.Pipelines.Schedules.Commands.ChangeStatusSchedule;
using TRT.Application.Pipelines.Schedules.Commands.SaveSchedule;
using TRT.Application.Pipelines.Schedules.Queries.GetScheduleById;
using TRT.Application.Pipelines.Schedules.Queries.GetSchedulesByFilter;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IMediator _mediator;

        public ScheduleController(ILogger<ScheduleController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("saveSchedule")]
        public async Task<IActionResult> SaveSchedule([FromBody] ScheduleDTO scheduleDTO)
        {
            try
            {
                var response = await _mediator.Send(new SaveScheduleCommand()
                {
                    ScheduleDTO = scheduleDTO
                });

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("getSchedulesByFilter")]
        public async Task<IActionResult> GetSchedulesByFilter([FromBody] GetSchedulesByFilterQuery getSchedulesByFilterQuery)
        {
            try
            {
                var response = await _mediator.Send(getSchedulesByFilterQuery);

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("getScheduleById")]
        public async Task<IActionResult> GetScheduleById(string id)
        {
            try
            {
                var response = await _mediator.Send(new GetScheduleByIdQuery(id));

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPut("changeStatusSchedule")]
        public async Task<IActionResult> ChangeStatusSchedule([FromBody] StatusChangeDTO statusChangeDto)
        {
            try
            {
                var response = await _mediator.Send(new ChangeStatusScheduleCommand()
                {
                    StatusChangeDTO = statusChangeDto
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
