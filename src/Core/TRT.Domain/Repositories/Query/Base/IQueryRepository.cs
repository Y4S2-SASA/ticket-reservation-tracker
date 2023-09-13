using System.Linq.Expressions;

namespace TRT.Domain.Repositories.Query.Base
{
    public interface IQueryRepository<T> where T : class
    {
        Task<List<T>> GetAll(CancellationToken cancellationToken);
        Task<T> GetById(string _id, CancellationToken cancellationToken);
        Task<IQueryable<T>> Query(Expression<Func<T, bool>> expression);
    }
}
