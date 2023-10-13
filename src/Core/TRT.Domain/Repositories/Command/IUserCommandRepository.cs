using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command.Base;
/*
 * File: IUserCommandRepository.cs
 * Author:Dunusinghe A.V/IT20025526
*/
namespace TRT.Domain.Repositories.Command
{
    public interface IUserCommandRepository : ICommandRepository<User>
    {
        // This method is responsible for updating user data
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    }
}
