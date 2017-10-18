namespace DapperExtensions.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public interface IQueryBuilder<T> where T : class//, IEntity
    {
        IQueryBuilder<T> Equals<TValue>(Expression<Func<T, TValue>> expression, TValue value);
        IQueryBuilder<T> NotEquals<TValue>(Expression<Func<T, TValue>> expression, TValue value);
        IQueryBuilder<T> LessThan<TValue>(Expression<Func<T, TValue>> expression, TValue value);
        IQueryBuilder<T> GreaterThan<TValue>(Expression<Func<T, TValue>> expression, TValue value);
        IQueryBuilder<T> In<TValue>(Expression<Func<T, TValue>> expression, params TValue[] values);
        IQueryBuilder<T> Like<TValue>(Expression<Func<T, TValue>> expression, TValue likeValue);

        //IQueryBuilder<T> SubQuery(IQueryBuilder<T> queryBuilder);
        //IPredicateGroup GetPredicate();
    }

    public class QueryBuilder<T> : IQueryBuilder<T> where T : class//, IEntity
    {
        private readonly PredicateGroup _predicateGroup;

        public QueryBuilder(GroupOperator groupOperator)
        {
            _predicateGroup = new PredicateGroup
            {
                Operator = groupOperator,
                Predicates = new List<IPredicate>()
            };
        }

        private IQueryBuilder<T> Find<TValue>(Expression<Func<T, TValue>> expression, object value, Operator @operator, bool not)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null || memberExpression.Expression.NodeType != ExpressionType.Parameter)
                throw new InvalidOperationException("Doing something fancy?");

            _predicateGroup.Predicates.Add(new FieldPredicate<T> { Not = not, Operator = @operator, PropertyName = memberExpression.Member.Name, Value = value });
            return this;
        }

        public IQueryBuilder<T> Equals<TValue>(Expression<Func<T, TValue>> expression, TValue value) { return Find(expression, value, Operator.Eq, false); }
        public IQueryBuilder<T> NotEquals<TValue>(Expression<Func<T, TValue>> expression, TValue value) { return Find(expression, value, Operator.Eq, true); }
        public IQueryBuilder<T> LessThan<TValue>(Expression<Func<T, TValue>> expression, TValue value) { return Find(expression, value, Operator.Lt, false); }
        public IQueryBuilder<T> GreaterThan<TValue>(Expression<Func<T, TValue>> expression, TValue value) { return Find(expression, value, Operator.Gt, false); }
        public IQueryBuilder<T> In<TValue>(Expression<Func<T, TValue>> expression, params TValue[] values) { return Find(expression, values, Operator.Eq, false); }
        public IQueryBuilder<T> Like<TValue>(Expression<Func<T, TValue>> expression, TValue likeValue) { return Find(expression, likeValue, Operator.Like, false); }
    }
}
