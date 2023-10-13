using MediatR;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Repositories.Query;
/*
 * File: ValidateUserNICQuery.cs
 * Purpose: Handle Validate User NIC
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.Application.Pipelines.Users.Queries.ValidateUserNIC
{
    public record ValidateUserNICQuery(string nic) : IRequest<ResultDTO>
    {
    }

    public class ValidateUserNICQueryHandler : IRequestHandler<ValidateUserNICQuery, ResultDTO>
    {
        private readonly IUserQueryRepository _userQueryRepository;
        public ValidateUserNICQueryHandler(IUserQueryRepository userQueryRepository)
        {
            this._userQueryRepository = userQueryRepository;
        }


        /// <summary>
        /// Handle Validate User NIC
        /// </summary>
        /// <param name="request">>Contains user input nic  </param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>ResultDTO</returns>
        public async Task<ResultDTO> Handle(ValidateUserNICQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _userQueryRepository.Query(x => x.NIC == request.nic)).FirstOrDefault();

                if(user is null)
                {
                    return ResultDTO.Success(string.Empty);
                }
                else
                {
                    return ResultDTO.Failure(new List<string>());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
