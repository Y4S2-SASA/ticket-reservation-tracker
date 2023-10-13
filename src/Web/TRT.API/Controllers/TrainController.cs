using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.DTOs.TrainDTOs;
using TRT.Application.Pipelines.Trains.Commands.ChangeTrainStatus;
using TRT.Application.Pipelines.Trains.Commands.SaveTrain;
using TRT.Application.Pipelines.Trains.Queries.GetTrainById;
using TRT.Application.Pipelines.Trains.Queries.GetTrainsByFilter;
/*
 * File: TrainController.cs
 * Purpose: Controller for managing the trains.
 * Author: Jayathilake S.M.D.A.R/IT20037338
*/
namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrainController : ControllerBase
    {
        private readonly ILogger<TrainController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mediator">The mediator.</param>
        public TrainController(ILogger<TrainController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("saveTrain")]
        public async Task<IActionResult> SaveTrain([FromBody] TrainDTO trainDTO)
        {
            try
            {
                var response = await _mediator.Send(new SaveTrainCommand()
                {
                    TrainDTO = trainDTO
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPut("changeTrainStatus")]
        public async Task<IActionResult> ChangeTrainStatus([FromBody] ChangeTrainStatusCommand changeTrainStatusCommand)
        {
            try
            {
                var response = await _mediator.Send(changeTrainStatusCommand);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("getTrainById/{id}")]
        public async Task<IActionResult> GetTrainById(string id)
        {
            try
            {
                var response = await _mediator.Send(new GetTrainByIdQuery(id));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("getTrainsByFilter")]
        public async Task<IActionResult> GetTrainsByFilter(GetTrainsByFilterQuery getTrainsByFilterQuery)
        {
            try
            {
                var response = await _mediator.Send(getTrainsByFilterQuery);

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
