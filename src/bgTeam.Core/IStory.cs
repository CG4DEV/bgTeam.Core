namespace bgTeam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IStory<in TStoryContext, TStoryResult>
        ////where TStoryContext : IStoryContext
    {
        /// <summary>
        /// Выполняет действия команды и вернуть результат
        /// </summary>
        /// <param name="commandContext">Контекст команды</param>
        TStoryResult Execute(TStoryContext context);

        Task<TStoryResult> ExecuteAsync(TStoryContext context);
    }
}
