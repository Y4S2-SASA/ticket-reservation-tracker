using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Command;
/*
 * File: SaveUserCommand.cs
 * Purpose: Save User 
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.Application.Pipelines.Users.Commands.SaveUserCommand
{
    public record SaveUserCommand(UserDTO UserData) : IRequest<ResultDTO>;
    

    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, ResultDTO>
    {
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly ILogger<SaveUserCommandHandler> _logger;
        public SaveUserCommandHandler
        (
            IUserCommandRepository userCommandRepository,
            ILogger<SaveUserCommandHandler> logger
        )
        {
            this._userCommandRepository = userCommandRepository;
            this._logger = logger;
        }

        /// <summary>
        /// Handle Save User 
        /// </summary>
        /// <param name="request">>Contains user details </param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>ResultDTO</returns>
        public async Task<ResultDTO> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = request.UserData.ToEntity();
                user.Status = Status.Activated;
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.UserData.Password);

                await _userCommandRepository.AddAsync(user, cancellationToken);

                return ResultDTO.Success
                    (
                        ResponseMessageConstant.USER_DETAILS_SAVE_SUCCESS_RESPONSE_MESSAGE
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                throw new ApplicationException(ResponseMessageConstant.COMMON_EXCEPTION_RESPONSE_MESSAGE);
            }
        }
    }
}
