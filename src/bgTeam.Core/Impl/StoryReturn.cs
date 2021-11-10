namespace bgTeam
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Выполняет историю и возвращает результат. Имеет возможность проверки доступа через <see cref="IStoryAccess"/>
    /// </summary>
    /// <typeparam name="TStoryContext">Тип контекста</typeparam>
    public class StoryReturn<TStoryContext> : IStoryReturn<TStoryContext>
    {
        private readonly IStoryFactory _factory;
        private readonly TStoryContext _context;
        private readonly IStoryAccess _access;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryReturn{TStoryContext}"/> class.
        /// </summary>
        /// <param name="access">Доступность к стои</param>
        /// <param name="factory">Фабрика истории</param>
        /// <param name="context">Контекст истории</param>
        public StoryReturn(
            IStoryAccess access,
            IStoryFactory factory,
            TStoryContext context)
        {
            _access = access;
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _context = context;
        }

        /// <inheritdoc/>
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
