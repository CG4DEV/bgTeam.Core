namespace DapperExtensions.Mapper.Sql
{
    using System.Collections.Generic;
    using bgTeam.DataAccess;
    using global::DapperExtensions.Mapper;

    public interface ISqlGenerator
    {
        IDapperExtensionsConfiguration Configuration { get; }

        string Select(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDictionary<string, object> parameters);

        string SelectPaged(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters);

        string SelectSet(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDictionary<string, object> parameters);

        string Count(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        string Insert(IClassMapper classMap);

        string Update(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        string Delete(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        string IdentitySql(IClassMapper classMap);

        string GetTableName(IClassMapper map);

        string GetColumnName(IClassMapper map, IPropertyMap property, bool includeAlias);

        string GetColumnName(IClassMapper map, string propertyName, bool includeAlias);

        bool SupportsMultipleStatements();
    }
}