using MongoDB.Bson;
using MongoDB.Driver;
using TRT.Domain.Repositories.Command.Base;
using TRT.Infrastructure.Data;

namespace TRT.Infrastructure.Repositories.Command.Base
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class
    {
        protected readonly TRTContext _context;
        public CommandRepository(TRTContext context)
        {
            this._context = context;
        }
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.GetCollection<T>(typeof(T).Name)
                          .InsertOneAsync(entity, cancellationToken: cancellationToken);
            return entity;
        }
        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(((dynamic)entity).Id));
            await _context.GetCollection<T>(typeof(T).Name)
                          .DeleteOneAsync(filter, cancellationToken);
        }
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(((dynamic)entity).Id));
            await _context.GetCollection<T>(typeof(T).Name)
                          .ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);


        }
    }
}
