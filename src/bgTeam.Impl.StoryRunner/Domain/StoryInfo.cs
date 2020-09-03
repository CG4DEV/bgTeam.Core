namespace bgTeam.Impl.StoryRunner.Domain
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Описание типа стори
    /// </summary>
    public class StoryInfo
    {
        /// <summary>
        /// Имя типа контекста story
        /// </summary>
        public string ContextName { get { return ContextType != null ? ContextType.Name : string.Empty; } }

        /// <summary>
        /// Имя типа story
        /// </summary>
        public string StoryName { get; set; }

        /// <summary>
        /// Тип story
        /// </summary>
        public Type StoryType { get; set; }

        /// <summary>
        /// Тип контекста
        /// </summary>
        public Type ContextType { get; set; }

        /// <summary>
        /// Запускаемый метода story
        /// </summary>
        public MethodInfo ExecuteMethodInfo { get; set; }
    }
}
