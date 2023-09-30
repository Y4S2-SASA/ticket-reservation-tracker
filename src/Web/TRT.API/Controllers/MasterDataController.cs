using MediatR;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.Pipelines.Stations.Queries.GetStationsMasterData;
using TRT.Application.Pipelines.Users.Queries.GetUserMasterData;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
