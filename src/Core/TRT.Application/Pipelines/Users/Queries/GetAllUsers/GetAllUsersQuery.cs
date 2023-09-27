using MediatR;
using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery : IRequest<List<UserDTO>>
    {
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
    {
        private readonly IUserQueryRepository _userQueryRepository;
        public GetAllUsersQueryHandler(IUserQueryRepository userQueryRepository)
        {
            this._userQueryRepository = userQueryRepository;
        }
        public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
           var query = await _userQueryRepository.GetAll(cancellationToken);

           var listOfUsers = query.Select(user => new UserDTO()
           {
                UserName = user.UserName,
                Email = user.Email,
               // Name = user.Name,
                NIC = user.NIC,
           }).ToList();

           return listOfUsers;
        }
    }
}
