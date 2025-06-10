using Microsoft.Extensions.DependencyInjection;

namespace Accidenta.Application.Common.Mediator.Dispatchers;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();
        return await handler.Handle(command, cancellationToken);
    }
}
