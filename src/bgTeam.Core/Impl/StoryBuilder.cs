namespace bgTeam
{
    /// <summary>
    /// Реализация класса для построений историй
    /// </summary>
    public class StoryBuilder : IStoryBuilder
    {
        private readonly IStoryFactory _factory;

        public StoryBuilder(IStoryFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Формирует историю для исполнения
        /// </summary>
        /// <typeparam name="TCommandContext"></typeparam>
        /// <param name="commandContext"></param>
        /// <returns></returns>
        public IStoryReturn<TStoryContext> Build<TStoryContext>(TStoryContext context)
        {
            return new StoryReturn<TStoryContext>(_factory, context);
        }
    }
}
