using Events.Infrastructure.Data.Context;

namespace Events.Infrastructure.Data.Commands;

public class DeferredCommandManager
{
    private readonly List<IDeferredCommand> _deferredCommands = [];
    private readonly object _lockObject = new();

    public void AddCommand(IDeferredCommand command)
    {
        lock (_lockObject)
        {
            _deferredCommands.Add(command);
        }
    }

    public async Task ExecuteCommandsAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        List<IDeferredCommand> commandsToExecute;
        lock (_lockObject)
        {
            commandsToExecute = new(_deferredCommands);
            _deferredCommands.Clear();
        }

        foreach (var command in commandsToExecute)
        {
            await command.ExecuteAsync(dbContext, ct);
        }
    }
}
