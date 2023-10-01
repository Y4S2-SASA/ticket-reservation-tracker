using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TRT.Application.DTOs.ScheduleDTOs;
using TRT.Application.Pipelines.Schedules.Commands.SaveSchedule;
using TRT.Application.Pipelines.Schedules.Queries.GetSchedulesByFilter;
using TRT.Application.Pipelines.Trains.Queries.GetTrainsByFilter;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

    }
}
