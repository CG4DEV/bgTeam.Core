namespace bgTeam.DataProducerCore.Common
{
    using bgTeam.Extensions;
    using bgTeam.ProduceMessages;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GroupConfig
    {
        public GroupConfig(IEnumerable<IDictionaryConfig> configs)
        {
            configs.CheckNullOrEmpty(nameof(configs));

            if (configs.Any(x => !x.GroupOrderBy.HasValue))
            {
                throw new ArgumentException($"У переданных элементов группы должна быть указана очередность исполнения GroupOrderBy");
            }

            configs = configs.OrderBy(x => x.GroupOrderBy.Value);

            var config = configs.First();
            if (configs.Any(x => x.GroupName != config.GroupName))
            {
                throw new ArgumentException("У переданных аргументов не единое имя группы");
            }

            GroupName = config.GroupName + "Group";
            DateFormatStart = config.DateFormatStart;
            Configs = configs
                .OrderBy(x => x.GroupOrderBy.Value)
                .Select(x => new GroupElement(x))
                .ToArray();
        }

        public string GroupName { get; private set; }

        public string DateFormatStart { get; private set; }

        public IEnumerable<GroupElement> Configs { get; private set; }
    }

    public class GroupElement
    {
        public GroupElement(IDictionaryConfig config)
        {
            EntityType = config.EntityType;
            EntityKey = config.EntityKey;
            GroupOrderBy = config.GroupOrderBy.Value;
            Sql = config.SqlString;
            DateChangeOffset = config.DateChangeOffset;
            Pool = config.Pool;
        }

        public string EntityType { get; private set; }

        public string EntityKey { get; private set; }

        public int GroupOrderBy { get; private set; }

        public string Sql { get; private set; }

        public int Pool { get; private set; }

        public int? DateChangeOffset { get; private set; }
    }
}
