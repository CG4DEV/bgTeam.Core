namespace bgTeam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StoryReturn<TStoryContext> : IStoryReturn<TStoryContext>
    {
        private readonly IStoryFactory _factory;
        private readonly TStoryContext _context;

        public StoryReturn(IStoryFactory factory, TStoryContext context)
        {
            _factory = factory;
            _context = context;
        }

        public TResult Return<TResult>()
        {
            return _factory.Create<TStoryContext, TResult>().Execute(_context);
        }

        public async Task<TResult> ReturnAsync<TResult>()
        {
            return await _factory.Create<TStoryContext, TResult>().ExecuteAsync(_context);
        }
    }
}
