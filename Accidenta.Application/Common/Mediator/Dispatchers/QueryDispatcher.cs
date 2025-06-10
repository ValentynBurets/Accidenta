using Microsoft.Extensions.DependencyInjection;

namespace Accidenta.Application.Common.Mediator.Dispatchers;

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        return await handler.Handle(query, cancellationToken);
    }
}
