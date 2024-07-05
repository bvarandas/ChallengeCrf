namespace ChallengeCrf.Domain.Interfaces;
public interface IUnitOfWork
{
    Task<bool> Commit(CancellationToken cancellationToken);
}
