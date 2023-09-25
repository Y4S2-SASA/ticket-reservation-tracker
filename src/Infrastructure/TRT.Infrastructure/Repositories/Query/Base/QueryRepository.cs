using MongoDB.Driver;
using System.Linq.Expressions;
using TRT.Domain.Repositories.Query.Base;
using TRT.Infrastructure.Data;

namespace TRT.Infrastructure.Repositories.Query.Base
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class
    {
        protected readonly TRTContext _context;
        public QueryRepository(TRTContext context)
        {
            this._context = context;
        }

        public async Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.GetCollection<T>(typeof(T).Name).Find(_ => true)
                                 .ToListAsync(cancellationToken);
        }

        public async Task<T> GetById(string _id, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq("_id", _id); 
            return await _context.GetCollection<T>(typeof(T).Name).Find(filter)
                                 .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IQueryable<T>> Query(Expression<Func<T, bool>> expression)
        {
            var collection = _context.GetCollection<T>(typeof(T).Name);
            var cursor = await collection.FindAsync(expression);

            return cursor.ToEnumerable().AsQueryable();
        }
    }
}
