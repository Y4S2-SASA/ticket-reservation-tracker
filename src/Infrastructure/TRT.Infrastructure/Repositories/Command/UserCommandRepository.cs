using MongoDB.Driver;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;
using TRT.Infrastructure.Data;
using TRT.Infrastructure.Repositories.Command.Base;
/*
 * File: UserCommandRepository.cs
 * Author:Dunusinghe A.V/IT20025526
*/
namespace TRT.Infrastructure.Repositories.Command
{
    public class UserCommandRepository
            : CommandRepository<User>, IUserCommandRepository
    {
        public UserCommandRepository(TRTContext context)
            : base(context)
        {

        }
        // This method is responsible for updating user data
        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            var filter = Builders<User>.Filter.Eq("_id", user.NIC);

            await _context.GetCollection<User>(typeof(User).Name)
                         .ReplaceOneAsync(filter, user, cancellationToken: cancellationToken);
        }
    }
}
