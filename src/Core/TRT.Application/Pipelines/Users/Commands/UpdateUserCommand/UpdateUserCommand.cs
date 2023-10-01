using MediatR;
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
        public UserUpdateCommandHandler(IUserCommandRepository userCommandRepository, IUserQueryRepository userQueryRepository)
        {
            this._userCommandRepository = userCommandRepository;
            this._userQueryRepository = userQueryRepository;
        }
        public async Task<ResultDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _userQueryRepository.Query(x=>x.NIC == request.UserDetail.NIC))
                           .FirstOrDefault();

                user = request.UserDetail.ToEntity(user);

                await _userCommandRepository.UpdateAsync(user, cancellationToken);

                return ResultDTO.Success(ResponseMessageConstant.USER_DETAILS_UPDATE_SUCCESS_RESPONSE_MESSAGE);
                
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
