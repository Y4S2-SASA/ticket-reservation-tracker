using MediatR;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.Pipelines.Dashboards.Queries.GetDashboardMasterData;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IMediator _mediator;

        public DashboardController(ILogger<DashboardController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("getDashboardMasterData")]
        public async Task<IActionResult> GetUserMasterData()
        {
            try
            {
                var response = await _mediator.Send(new GetDashboardMasterDataQuery());
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
