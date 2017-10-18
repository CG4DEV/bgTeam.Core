namespace bgTeam.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class MaybeExtension
    {
        public static TInput IfLess<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }

            return evaluator(o) ? o : null;
        }

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

        public static IList<T> AddNotNull<T>(this IList<T> @this, T item)
            where T : class
        {
            if (@this != null && item != null)
            {
                @this.Add(item);
            }

            return @this;
        }

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
