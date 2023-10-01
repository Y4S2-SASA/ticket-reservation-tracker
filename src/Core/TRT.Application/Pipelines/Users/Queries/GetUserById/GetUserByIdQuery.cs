using MediatR;
using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Repositories.Query;

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
