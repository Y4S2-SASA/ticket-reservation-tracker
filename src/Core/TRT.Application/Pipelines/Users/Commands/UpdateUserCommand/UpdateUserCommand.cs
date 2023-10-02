using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Users.Commands.UpdateUserCommand
{
    public record UpdateUserCommand(UserDTO UserDetail) : IRequest<ResultDTO>;
   
    public class UserUpdateCommandHandler : IRequestHandler<UpdateUserCommand, ResultDTO>
    {
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly ILogger<UserUpdateCommandHandler> _logger;
        public UserUpdateCommandHandler
        (
            IUserCommandRepository userCommandRepository, 
            IUserQueryRepository userQueryRepository,
            ILogger<UserUpdateCommandHandler> logger
        )
        {
            this._userCommandRepository = userCommandRepository;
            this._userQueryRepository = userQueryRepository;
            this._logger = logger;
        }
        public async Task<ResultDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _userQueryRepository.Query(x=>x.NIC == request.UserDetail.NIC))
                           .FirstOrDefault();

                user = request.UserDetail.ToEntity(user);

                await _userCommandRepository.UpdateUserAsync(user, cancellationToken);

                return ResultDTO.Success(ResponseMessageConstant.USER_DETAILS_UPDATE_SUCCESS_RESPONSE_MESSAGE);
                
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex.Message, ex);

                throw new ApplicationException(ResponseMessageConstant.COMMON_EXCEPTION_RESPONSE_MESSAGE);
            }
        }
    }
}
