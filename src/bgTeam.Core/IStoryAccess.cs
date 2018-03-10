namespace bgTeam
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IStoryAccess
    {
        void CheckAccess<TStoryContext, TResult>(IStory<TStoryContext, TResult> story);

        Task CheckAccessAsync<TStoryContext, TResult>(IStory<TStoryContext, TResult> story);
    }
}
