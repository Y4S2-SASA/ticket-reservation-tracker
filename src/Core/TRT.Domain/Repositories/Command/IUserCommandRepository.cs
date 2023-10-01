using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command.Base;

namespace TRT.Domain.Repositories.Command
{
    public interface IUserCommandRepository : ICommandRepository<User>
    {
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    }
}
