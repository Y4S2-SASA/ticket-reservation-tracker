using MediatR;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.Pipelines.Users.Commands.DeactiveUserCommand;
using TRT.Application.Pipelines.Users.Commands.ReactiveUserCommand;
using TRT.Application.Pipelines.Users.Commands.SaveUserCommand;
using TRT.Application.Pipelines.Users.Commands.UpdateUserCommand;
using TRT.Application.Pipelines.Users.Queries.GetAllUsers;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(ILogger<UserController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("saveUser")]
        public async Task<IActionResult> SaveUser([FromBody] SaveUserCommand saveUserCommand)
        {
            var response = await _mediator.Send(saveUserCommand);

            return Ok(response);
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand updateUserCommand)
        {
            var response = await _mediator.Send(updateUserCommand);

            return Ok(response);
        }

        [HttpPut("deactiveUser")]
        public async Task<IActionResult> DeactiveUser(string nic)
        {
            var response = await _mediator.Send(new DeactiveUserCommand(nic));

            return Ok(response);
        }

        [HttpPut("reactiveUser")]
        public async Task<IActionResult> ReactiveUser(string nic)
        {
            var response = await _mediator.Send(new ReactiveUserCommand(nic));

            return Ok(response);
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _mediator.Send(new GetAllUsersQuery());

            return Ok(response);
        }
    }
}
