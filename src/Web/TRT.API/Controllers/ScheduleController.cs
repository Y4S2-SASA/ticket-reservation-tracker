﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.ScheduleDTOs;
using TRT.Application.Pipelines.Reservations.Queries.GetAvailableTrainSeatCount;
using TRT.Application.Pipelines.Schedules.Commands.ChangeStatusSchedule;
using TRT.Application.Pipelines.Schedules.Commands.SaveSchedule;
using TRT.Application.Pipelines.Schedules.Queries.GetScheduleById;
using TRT.Application.Pipelines.Schedules.Queries.GetSchedulePrice;
using TRT.Application.Pipelines.Schedules.Queries.GetSchedulesByFilter;
using TRT.Application.Pipelines.Schedules.Queries.GetScheduleTrainsByDestinationId;
using TRT.Application.Pipelines.Schedules.Queries.GetScheduleTrainsData;
using TRT.Application.Pipelines.Schedules.Queries.GetSchudulesSubStationsByTrainId;
/*
 * File: ScheduleController.cs
 * Purpose: Controller for managing the Schedules.
 * Author: Perera M.S.D/1IT20020262
*/
namespace TRT.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mediator">The mediator.</param>
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

        [HttpGet("getScheduleTrainsByDestinationId/{destinationId}")]
        public async Task<IActionResult> GetScheduleTrainsByDestinationId(string destinationId)
        {
            try
            {
                var response = await _mediator.Send(new GetScheduleTrainsByDestinationIdQuery(destinationId));

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("getSchudulesSubStationsByTrainId/{trainId}")]
        public async Task<IActionResult> GetSchudulesSubStationsByTrainId(string trainId)
        {
            try
            {
                var response = await _mediator.Send(new GetSchudulesSubStationsByTrainIdQuery(trainId));

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("getScheduleTrainsData")]
        public async Task<IActionResult> GetScheduleTrainsData(GetScheduleTrainsDataQuery getScheduleTrainsDataQuery)
        {
            try
            {
                var response = await _mediator.Send(getScheduleTrainsDataQuery);

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("getAvailableTrainSeatCount")]
        public async Task<IActionResult> GetAvailableTrainSeatCount(GetAvailableTrainSeatCountQuery getAvailableTrainSeatCountQuery)
        {
            try
            {
                var response = await _mediator.Send(getAvailableTrainSeatCountQuery);

                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("getSchedulePrice")]
        public async Task<IActionResult> GetSchedulePrice(GetSchedulePriceQuery getSchedulePriceQuery)
        {
            try
            {
                var response = await _mediator.Send(getSchedulePriceQuery);

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
