namespace bgTeam.DataAccess.Impl
{
    using System.Threading.Tasks;

    internal class CommandReturn<TCommandContext> : ICommandReturn<TCommandContext>
    {
        private readonly TCommandContext _commandContext;
        private readonly ICommandFactory _commandFactory;

        public CommandReturn(ICommandFactory commandFactory, TCommandContext context)
        {
            _commandContext = context;
            _commandFactory = commandFactory;
        }

        public void Execute()
        {
            _commandFactory.Create<TCommandContext>()
                .Execute(_commandContext);
        }

        public async Task ExecuteAsync()
        {
            await _commandFactory.Create<TCommandContext>()
                .ExecuteAsync(_commandContext);
        }

        public TResult Return<TResult>()
        {
            return _commandFactory.Create<TCommandContext, TResult>()
                .Execute(_commandContext);
        }

        public async Task<TResult> ReturnAsync<TResult>()
        {
            return await _commandFactory.Create<TCommandContext, TResult>()
                .ExecuteAsync(_commandContext);
        }
    }
}