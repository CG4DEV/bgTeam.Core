namespace bgTeam.StoryRunner.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class StoryProcessorRepository : IStoryProcessorRepository
    {
        private readonly IDictionary<string, StoryInfo> _storyInfos;

        public StoryProcessorRepository()
            : this(Assembly.GetExecutingAssembly())
        {
        }

        public StoryProcessorRepository(Type type)
            : this(type.Assembly)
        {
        }

        public StoryProcessorRepository(Assembly assembly)
        {
            _storyInfos = Init(assembly);
        }

        private static IDictionary<string, StoryInfo> Init(Assembly assembly)
        {
            var types = assembly.GetTypes();

            var intefaceType = typeof(IStory<,>);
            var dictionary = types
                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == intefaceType))
                .Select(x => new
                {
                    Inteface = x.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == intefaceType),
                    StoryName = x.Name,
                })
                .Select(x => new StoryInfo()
                {
                    ContextType = x.Inteface.GenericTypeArguments.First(),
                    ExecuteMethodInfo = x.Inteface.GetMethod("ExecuteAsync"),
                    StoryType = x.Inteface,
                    StoryName = x.StoryName,
                })
                .ToDictionary(x => x.ContextName);

            return new ConcurrentDictionary<string, StoryInfo>(dictionary, StringComparer.InvariantCultureIgnoreCase);
        }

        public StoryInfo Get(string name)
        {
            if (_storyInfos.ContainsKey(name))
            {
                return _storyInfos[name];
            }

            throw new KeyNotFoundException($"Story not found with context {name}");
        }
    }
}
