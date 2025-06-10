namespace Accidenta.Application.Common.Mediator;

public interface ICommandHandler<in TCommand, TCommandResult>
{
    Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken);
}
