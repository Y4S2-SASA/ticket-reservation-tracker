using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Users.Commands.ChangeUserStatus
{
    public record ChangeUserStatusCommand : IRequest<ResultDTO>
    {
        public string Nic { get; set; }
        public Status Status { get; set; }
    }

    public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, ResultDTO>
    {
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly ILogger<ChangeUserStatusCommandHandler> _logger;
        public ChangeUserStatusCommandHandler
        (
            IUserCommandRepository userCommandRepository, 
            IUserQueryRepository userQueryRepository,
            ILogger<ChangeUserStatusCommandHandler> logger
        )
        {
            this._userCommandRepository = userCommandRepository;
            this._userQueryRepository = userQueryRepository;
            this._logger = logger;
        }
        public async Task<ResultDTO> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _userQueryRepository.Query(x => x.NIC == request.Nic))
                          .FirstOrDefault();

                if (user is null)
                {
                    return ResultDTO.Failure(new List<string>()
                    {
                        ResponseMessageConstant.USER_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE
                    });
                }

                user.Status = request.Status;

                await _userCommandRepository.UpdateAsync(user, cancellationToken);

                return ResultDTO.Success
                (
                   string.Format
                   (
                       ResponseMessageConstant.USER_STATUS_CHANGE_SUCCESS_RESPONSE_MESSAGE, 
                       $"{user.FistName} " +
                       $"{user.LastName}", 
                       EnumHelper.GetEnumDescription(request.Status)
                  )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseMessageConstant.COMMON_EXCEPTION_RESPONSE_MESSAGE);
                throw;
            }
        }
    }
}
