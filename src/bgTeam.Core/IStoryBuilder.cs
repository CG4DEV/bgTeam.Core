namespace bgTeam
{
    /// <summary>
    /// Интерфейс для построений историй
    /// </summary>
    public interface IStoryBuilder
    {
        /// <summary>
        /// Формирует историю для исполнения
        /// </summary>
        /// <typeparam name="TCommandContext"></typeparam>
        /// <param name="commandContext"></param>
        /// <returns></returns>
        IStoryReturn<TStoryContext> Build<TStoryContext>(TStoryContext context);
            //where TCommandContext : ICommandContext;
    }
}
