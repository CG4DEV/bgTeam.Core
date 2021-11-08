using System.Threading;
using System.Threading.Tasks;

namespace bgTeam
{
    /// <summary>
    /// Интерфейс истории
    /// </summary>
    /// <typeparam name="TStoryContext">Тип контекста</typeparam>
    /// <typeparam name="TStoryResult">Тип возвращаемого значения</typeparam>
    public interface IStory<in TStoryContext, TStoryResult>
    {
        /// <summary>
        /// Асинхронно выполняет действия истории и возвращает результат
        /// </summary>
        /// <param name="context">Контекст истории</param>
        /// <param name="ct"></param>
        Task<TStoryResult> ExecuteAsync(TStoryContext context, CancellationToken ct = default);
    }
}
