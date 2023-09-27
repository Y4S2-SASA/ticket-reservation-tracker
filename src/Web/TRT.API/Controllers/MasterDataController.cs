using MediatR;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.Pipelines.Users.Queries.GetUserMasterData;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public MasterDataController(ILogger<UserController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("getUserMasterData")]
        public async Task<IActionResult> GetUserMasterData()
        {
            var response = await _mediator.Send(new GetUserMasterDataQuery());

            return Ok(response);
        }
    }
}
