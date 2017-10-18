namespace bgTeam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
