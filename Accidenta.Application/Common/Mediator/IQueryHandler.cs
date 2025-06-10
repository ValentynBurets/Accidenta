namespace Accidenta.Application.Common.Mediator;

public interface IQueryHandler<in TQuery, TQueryResult>
{
    Task<TQueryResult> Handle(TQuery query, CancellationToken cancellationToken);
}
