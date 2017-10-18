namespace bgTeam.ProcessMessages
{
    using bgTeam.Extensions;
    using bgTeam.Queues;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IEntityMapService
    {
        EntityMap CreateEntityMap(IQueueMessage message);
    }

    public class DefaultMapService : IEntityMapService
    {
        public EntityMap CreateEntityMap(IQueueMessage message)
        {
            var settings = new JsonSerializerSettings()
            {
                DateParseHandling = DateParseHandling.None,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };

            var obj = JsonConvert.DeserializeObject<dynamic>(message.Body, settings);
            var map = ((IDictionary<string, JToken>)obj)
                .ToDictionary(
                    x => x.Key,
                    x => string.IsNullOrWhiteSpace(x.Value.ToString()) ? null : x.Value.ToObject<object>()
                );
            return new EntityMap(map);
        }
    }

    public class EntityMap
    {
        private const string ENTITY_TYPE_NAME = "EntityType";
        private const string ENTITY_KEY_NAME = "EntityKey";
        private const string ENTITY_KEY_NAME_DEFAULT = "Id";

        public EntityMap(Dictionary<string, object> map)
        {
            // Создаём регистронезависимый словарь
            Properties = new Dictionary<string, object>(map.CheckNull(nameof(map)), StringComparer.OrdinalIgnoreCase);

            if (!map.ContainsKey(ENTITY_TYPE_NAME))
            {
                throw new ArgumentException($"Отсутствует поле {ENTITY_TYPE_NAME}");
            }

            TypeName = map[ENTITY_TYPE_NAME].ToString();
            KeyName = map.ContainsKey(ENTITY_KEY_NAME) ? map[ENTITY_KEY_NAME].ToString() : ENTITY_KEY_NAME_DEFAULT;
        }

        public string TypeName { get; private set; }

        public string KeyName { get; private set; }

        public object KeyValue => Properties[KeyName];

        public Dictionary<string, object> Properties { get; private set; }

        public IEnumerable<string> PropertyNames => Properties.Keys.Except(new[] { ENTITY_TYPE_NAME, ENTITY_KEY_NAME }).ToArray();
    }
}
