namespace bgTeam
{
    using System;
    using System.Threading.Tasks;

    public class StoryReturn<TStoryContext> : IStoryReturn<TStoryContext>
    {
        private readonly IStoryFactory _factory;
        private readonly TStoryContext _context;
        private readonly IStoryAccess _access;

        public StoryReturn(
            IStoryAccess access,
            IStoryFactory factory,
            TStoryContext context)
        {
            _access = access;
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _context = context;
        }

        /// <summary>
        /// Выполнить историю, и вернуть результат
        /// </summary>
        public TResult Return<TResult>()
        {
            var story = _factory.Create<TStoryContext, TResult>();

            if (_access != null)
            {
                _access.CheckAccess(story);
            }

            return story.Execute(_context);
        }

        /// <summary>
        /// Выполнить историю асинхронно, и вернуть результат
        /// </summary>
        public async Task<TResult> ReturnAsync<TResult>()
        {
            var story = _factory.Create<TStoryContext, TResult>();

            if (_access != null)
            {
                await _access.CheckAccessAsync(story);
            }

            return await story.ExecuteAsync(_context);
        }
    }
}
