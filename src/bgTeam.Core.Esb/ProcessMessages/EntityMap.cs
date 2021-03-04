namespace bgTeam.ProcessMessages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using bgTeam.Extensions;

    /// <summary>
    /// Содержит основную информацию об объекте из очереди.
    /// </summary>
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

        /// <summary>
        /// Тип объекта из очереди.
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        /// Ключевое поле.
        /// </summary>
        public string KeyName { get; private set; }

        /// <summary>
        /// Значение ключевого поля.
        /// </summary>
        public object KeyValue => Properties[KeyName];

        /// <summary>
        /// Словарь имя-значение всех полей объекта из очереди.
        /// </summary>
        public Dictionary<string, object> Properties { get; private set; }

        /// <summary>
        /// Имена всех свойств объекта из очереди.
        /// </summary>
        public IEnumerable<string> PropertyNames => GetPropertyNames();

        private IEnumerable<string> GetPropertyNames()
        {
            return Properties.Keys.Except(new[] { ENTITY_TYPE_NAME, ENTITY_KEY_NAME }).ToArray();
        }
    }
}
