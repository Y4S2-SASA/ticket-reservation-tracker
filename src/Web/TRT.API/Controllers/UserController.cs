using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.Pipelines.Users.Commands.ChangeUserStatus;
using TRT.Application.Pipelines.Users.Commands.SaveUserCommand;
using TRT.Application.Pipelines.Users.Commands.UpdateUserCommand;
using TRT.Application.Pipelines.Users.Queries.GetAllUsers;
using TRT.Application.Pipelines.Users.Queries.GetAllUsersByFilter;
using TRT.Domain.Constants;

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

        [Authorize(Roles = AuthorizedRoles.BackOffice)]
        [HttpPut("changeUserStatus")]
        public async Task<IActionResult> ChangeUserStatus(ChangeUserStatusCommand changeUserStatusCommand)
        {
            var response = await _mediator.Send(changeUserStatusCommand);

            return Ok(response);
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _mediator.Send(new GetAllUsersQuery());

            return Ok(response);
        }

        [HttpPost("getAllUsersByFilter")]
        public async Task<IActionResult> GetAllUsersByFilter(GetAllUsersByFilterQuery getUsersByFilterQuery)
        {
            var response = await _mediator.Send(getUsersByFilterQuery);

            return Ok(response);
        }
    }
}
