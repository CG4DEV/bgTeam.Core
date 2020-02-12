namespace bgTeam.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Содержит расширения для манипуляции коллекциями и объектами
    /// </summary>
    public static class MaybeExtension
    {
        // May be correct name is IfTrue?
        public static TInput IfLess<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }

            return evaluator(o) ? o : null;
        }

        // May be correct name is IfFalse?
        public static TInput IfUnless<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }

            return evaluator(o) ? null : o;
        }

        public static TResult With<TInput, TResult>(this TInput @this, Func<TInput, TResult> func)
            where TInput : class
        {
            if (@this != null)
            {
                return func(@this);
            }

            return default(TResult);
        }

        public static TResult With<TInput, TResult>(this TInput? @this, Func<TInput, TResult> func)
            where TInput : struct
        {
            if (@this.HasValue)
            {
                return func(@this.Value);
            }

            return default(TResult);
        }

        public static TResult Return<TInput, TResult>(this TInput @this, Func<TInput, TResult> func, TResult @default)
            where TInput : class
        {
            if (@this != null)
            {
                return func(@this);
            }

            return @default;
        }

        public static TResult Return<TInput, TResult>(this TInput? @this, Func<TInput, TResult> func, TResult @default)
            where TInput : struct
        {
            if (@this.HasValue)
            {
                return func(@this.Value);
            }

            return @default;
        }

        public static void Do<TInput>(this TInput @this, Action<TInput> func)
            where TInput : class
        {
            if (@this != null)
            {
                func(@this);
            }
        }

        public static void Do<TInput>(this TInput? @this, Action<TInput> func)
            where TInput : struct
        {
            if (@this.HasValue)
            {
                func(@this.Value);
            }
        }

        public static void DoVal<TInput>(this TInput @this, Action<TInput> func)
            where TInput : struct
        {
            func(@this);
        }

        /// <summary>
        /// Цикл Foreach, который можно встроить в Linq-выражение
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="this">Коллекция</param>
        /// <param name="func">Действие, котрое будет производится над элементами коллекции</param>
        /// <returns>Возвращает переданную коллекцию</returns>
        public static IEnumerable<T> DoForEach<T>(this IEnumerable<T> @this, Action<T> func)
        {
            if (@this != null)
            {
                foreach (T item in @this)
                {
                    func(item);
                }
            }

            return @this;
        }

        /// <summary>
        /// Добавляет новый элемент в коллекцию, если он не null
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="this">Коллекция</param>
        /// <param name="item">Добавляемый объект</param>
        /// <returns>Переданная коллекция</returns>
        public static IList<T> AddNotNull<T>(this IList<T> @this, T item)
            where T : class
        {
            if (@this != null && item != null)
            {
                @this.Add(item);
            }

            return @this;
        }

        /// <summary>
        /// Добавляет новый элемент в коллекцию, если он не null
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="this">Коллекция</param>
        /// <param name="item">Добавляемый объект</param>
        /// <returns>Переданная коллекция</returns>
        public static IList<T> AddNotNull<T>(this IList<T> @this, T? item)
            where T : struct
        {
            if (@this != null && item.HasValue)
            {
                @this.Add(item.Value);
            }

            return @this;
        }
    }
}
