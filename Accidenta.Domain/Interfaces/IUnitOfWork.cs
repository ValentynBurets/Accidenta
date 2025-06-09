namespace Accidenta.Domain.Interfaces;

public interface IUnitOfWork
{
    IContactRepository Contacts { get; }
    IAccountRepository Accounts { get; }
    IIncidentRepository Incidents { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}
