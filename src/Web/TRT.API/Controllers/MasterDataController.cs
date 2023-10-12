using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.Pipelines.Stations.Queries.GetAllStationMasterData;
using TRT.Application.Pipelines.Stations.Queries.GetStationsMasterData;
using TRT.Application.Pipelines.Trains.Queries.GetTrainDetailMasterData;
using TRT.Application.Pipelines.Trains.Queries.GetTrainMasterDataByName;
using TRT.Application.Pipelines.Users.Queries.GetUserMasterData;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MasterDataController : ControllerBase
    {
        private readonly ILogger<MasterDataController> _logger;
        private readonly IMediator _mediator;

        public MasterDataController(ILogger<MasterDataController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("getUserMasterData")]
        public async Task<IActionResult> GetUserMasterData()
        {
            try
            {
                var response = await _mediator.Send(new GetUserMasterDataQuery());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("getStationsMasterData")]
        public async Task<IActionResult> GetStationsMasterData
        (
            GetStationsMasterDataQuery getStationsMasterDataQuery
        )
        {
           
            try
            {
                var response = await _mediator.Send(getStationsMasterDataQuery);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("getTrainDetailMasterData")]
        public async Task<IActionResult> GetTrainDetailMasterData()
        {
            try
            {
                var response = await _mediator.Send(new GetTrainDetailMasterDataQuery());

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("getTrainMasterData")]
        public async Task<IActionResult> GetTrainMasterData(GetTrainMasterDataByNameQuery getTrainMasterData)
        {
            try
            {
                var response = await _mediator.Send(getTrainMasterData);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("getAllStationMasterData")]
        public async Task<IActionResult> GetAllStationMasterData()
        {
            try
            {
                var response = await _mediator.Send(new GetAllStationMasterDataQuery());

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
