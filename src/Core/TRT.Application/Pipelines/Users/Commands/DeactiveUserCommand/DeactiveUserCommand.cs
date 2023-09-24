using MediatR;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Users.Commands.DeactiveUserCommand
{
    public record DeactiveUserCommand(string nic) : IRequest<ResultDTO>;

    public class DeactiveUserCommandHandler : IRequestHandler<DeactiveUserCommand, ResultDTO>
    {
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        public DeactiveUserCommandHandler(IUserCommandRepository userCommandRepository, IUserQueryRepository userQueryRepository)
        {
            this._userCommandRepository = userCommandRepository;
            this._userQueryRepository = userQueryRepository;
        }
        public async Task<ResultDTO> Handle(DeactiveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _userQueryRepository.Query(x => x.NIC == request.nic))
                           .FirstOrDefault();

                user.IsActive = false;

                await _userCommandRepository.UpdateAsync(user, cancellationToken);

                return ResultDTO.Success
                       (
                            string.Format(ResponseMessageConstant.USER_DETAILS_DEACTIVE_SUCCESS_RESPONSE_MESSAGE, user.Name)
                       );

            }
            catch (Exception ex)
            {
                return ResultDTO.Failure(new List<string>()
                {
                   ResponseMessageConstant.COMMON_EXCEPTION_RESPONSE_MESSAGE
                });
            }
        }
    }
}
