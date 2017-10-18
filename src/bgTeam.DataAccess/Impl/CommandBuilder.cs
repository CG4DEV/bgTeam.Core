namespace bgTeam.DataAccess.Impl
{
    using System.Threading.Tasks;

    /// <summary>
    /// Стандартная реализация интерефейса ICommandBuilder
    /// </summary>
    public class CommandBuilder : ICommandBuilder
    {
        private readonly ICommandFactory _commandFactory;

        public CommandBuilder(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public void Execute<TCommandContext>(TCommandContext commandContext)
        {
            _commandFactory.Create<TCommandContext>().Execute(commandContext);
        }

        public async Task ExecuteAsync<TCommandContext>(TCommandContext commandContext)
        {
            await _commandFactory.Create<TCommandContext>().ExecuteAsync(commandContext);
        }

        public ICommandReturn<TCommandContext> Build<TCommandContext>(TCommandContext commandContext)
        {
            return new CommandReturn<TCommandContext>(_commandFactory, commandContext);
        }

        public async Task<TResult> ExecuteAsync<TCommandContext, TResult>(TCommandContext commandContext)
        {
            return await _commandFactory.Create<TCommandContext, TResult>().ExecuteAsync(commandContext);
        }
    }
}