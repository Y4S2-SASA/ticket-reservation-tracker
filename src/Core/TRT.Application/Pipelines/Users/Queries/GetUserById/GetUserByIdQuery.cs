using MediatR;
using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Repositories.Query;
/*
 * File: GetUserByIdQuery.cs
 * Purpose: Handle GetUser By Id 
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.Application.Pipelines.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(string id) : IRequest<UserDTO>
    {
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly IUserQueryRepository _userQueryRepository;
        public GetUserByIdQueryHandler(IUserQueryRepository userQueryRepository)
        {
            this._userQueryRepository = userQueryRepository;
        }

        /// <summary>
        /// Handle  GetUser By Id 
        /// </summary>
        /// <param name="request">>Contains user id </param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>User details</returns>
        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _userQueryRepository.Query(x=>x.NIC == request.id))
                            .FirstOrDefault();

                return user.ToDto();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
