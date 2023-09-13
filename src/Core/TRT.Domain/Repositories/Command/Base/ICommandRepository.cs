namespace TRT.Domain.Repositories.Command.Base
{
    public interface ICommandRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
