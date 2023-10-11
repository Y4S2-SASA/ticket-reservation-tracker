using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRT.Application.DTOs.UserDTOs;
using TRT.Application.Pipelines.Users.Commands.ChangeUserStatus;
using TRT.Application.Pipelines.Users.Commands.SaveUserCommand;
using TRT.Application.Pipelines.Users.Commands.UpdateUserCommand;
using TRT.Application.Pipelines.Users.Queries.GetAllUsersByFilter;
using TRT.Application.Pipelines.Users.Queries.GetUserById;
using TRT.Application.Pipelines.Users.Queries.ValidateUserNIC;
using TRT.Domain.Constants;

namespace TRT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(ILogger<UserController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("saveUser")]
        public async Task<IActionResult> SaveUser([FromBody] UserDTO userDTO)
        {
            try
            {
                var response = await _mediator.Send(new SaveUserCommand(userDTO));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO)
        {
            
            try
            {
                var response = await _mediator.Send(new UpdateUserCommand(userDTO));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [Authorize(Roles = AuthorizedRoles.BackOffice)]
        [HttpPut("changeUserStatus")]
        public async Task<IActionResult> ChangeUserStatus(ChangeUserStatusCommand changeUserStatusCommand)
        {
            try
            {
                var response = await _mediator.Send(changeUserStatusCommand);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("getAllUsersByFilter")]
        public async Task<IActionResult> GetAllUsersByFilter(GetAllUsersByFilterQuery getUsersByFilterQuery)
        {
            try
            {
                var response = await _mediator.Send(getUsersByFilterQuery);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var response = await _mediator.Send(new GetUserByIdQuery(id));

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [HttpPost("validateUserNIC")]
        public async Task<IActionResult> ValidateUserNIC(ValidateUserNICQuery validateUserNICQuery)
        {
            try
            {
                var response = await _mediator.Send(validateUserNICQuery);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        [Authorize(Roles = AuthorizedRoles.Traveler)]
        [HttpPut("deactiveTravelerAccount")]
        public async Task<IActionResult> DeactiveTravelerAccount(ChangeUserStatusCommand changeUserStatusCommand)
        {
            try
            {
                if(changeUserStatusCommand.Status == Domain.Enums.Status.Deactivated)
                {
                    var response = await _mediator.Send(changeUserStatusCommand);

                    return Ok(response);
                }
                else
                {
                    return BadRequest();
                }
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
