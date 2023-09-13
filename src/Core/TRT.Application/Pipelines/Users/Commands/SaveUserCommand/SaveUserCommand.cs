using MediatR;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;

namespace TRT.Application.Pipelines.Users.Commands.SaveUserCommand
{
    public record SaveUserCommand : IRequest<bool>
    {
        public string NIC { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, bool>
    {
        private readonly IUserCommandRepository userCommandRepository;
        public SaveUserCommandHandler(IUserCommandRepository _userCommandRepository)
        {
            this.userCommandRepository = _userCommandRepository;
        }
        public async Task<bool> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = new User()
                {
                    NIC = request.NIC,
                    Name = request.Name,
                    Email = request.Email,
                    UserName = request.UserName,
                    PasswordHash = request.PasswordHash

                };

                await userCommandRepository.AddAsync(user, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
