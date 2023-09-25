using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Users.Commands.ReactiveUserCommand
{
    public record ReactiveUserCommand(string nic) : IRequest<ResultDTO>
    {
    }

    public class ReactiveUserCommandHandler : IRequestHandler<ReactiveUserCommand, ResultDTO>
    {
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        public ReactiveUserCommandHandler(IUserCommandRepository userCommandRepository, IUserQueryRepository userQueryRepository)
        {
            this._userCommandRepository = userCommandRepository;
            this._userQueryRepository = userQueryRepository;
        }
        public async Task<ResultDTO> Handle(ReactiveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _userQueryRepository.Query(x => x.NIC == request.nic))
                           .FirstOrDefault();

                user.IsActive = true;

                await _userCommandRepository.UpdateAsync(user, cancellationToken);

                return ResultDTO.Success
                       (
                            string.Format(ResponseMessageConstant.USER_DETAILS_REACTIVE_SUCCESS_RESPONSE_MESSAGE, user.Name)
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
