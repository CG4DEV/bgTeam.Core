namespace bgTeam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StoryBuilder : IStoryBuilder
    {
        private readonly IStoryFactory _factory;

        public StoryBuilder(IStoryFactory factory)
        {
            _factory = factory;
        }

        public IStoryReturn<TStoryContext> Build<TStoryContext>(TStoryContext context)
        {
            return new StoryReturn<TStoryContext>(_factory, context);
        }
    }
}
