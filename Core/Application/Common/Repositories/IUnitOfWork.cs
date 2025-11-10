namespace Application.Common.Repositories;
public interface IUnitOfWork
{
    void Save();
    Task SaveAsync(CancellationToken cancellationToken = default);
}