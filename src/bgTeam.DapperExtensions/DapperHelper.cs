namespace DapperExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using DapperExtensions.Builder;
    using DapperExtensions.Mapper;
    using Mapper.Sql;

    public static class DapperHelper
    {
        private readonly static object _lock = new object();

        private static Func<IDapperExtensionsConfiguration, IDapperImplementor> _instanceFactory;
        private static IDapperImplementor _instance;
        private static IDapperExtensionsConfiguration _configuration;
        
        /// <summary>
        /// Gets or sets the default class mapper to use when generating class maps. If not specified, AutoClassMapper<T> is used.
        /// DapperExtensions.Configure(Type, IList<Assembly>, ISqlDialect) can be used instead to set all values at once
        /// </summary>
        public static Type DefaultMapper
        {
            get
            {
                return _configuration.DefaultMapper;
            }

            set
            {
                Configure(value, _configuration.MappingAssemblies, _configuration.Dialect, _configuration.IdentityColumn);
            }
        }

        /// <summary>
        /// Gets or sets the type of sql to be generated.
        /// DapperExtensions.Configure(Type, IList<Assembly>, ISqlDialect) can be used instead to set all values at once
        /// </summary>
        public static ISqlDialect SqlDialect
        {
            get
            {
                return _configuration.Dialect;
            }

            set
            {
                Configure(_configuration.DefaultMapper, _configuration.MappingAssemblies, value, _configuration.IdentityColumn);
            }
        }

        public static string IdentityColumn
        {
            get
            {
                return _configuration.IdentityColumn;
            }

            set
            {
                Configure(_configuration.DefaultMapper, _configuration.MappingAssemblies, _configuration.Dialect, value);
            }
        }

        /// <summary>
        /// Get or sets the Dapper Extensions Implementation Factory.
        /// </summary>
        public static Func<IDapperExtensionsConfiguration, IDapperImplementor> InstanceFactory
        {
            get
            {
                if (_instanceFactory == null)
                {
                    _instanceFactory = config => new DapperImplementor(new SqlGeneratorImpl(config));
                }

                return _instanceFactory;
            }
            set
            {
                _instanceFactory = value;
                Configure(_configuration.DefaultMapper, _configuration.MappingAssemblies, _configuration.Dialect, _configuration.IdentityColumn);
            }
        }

        /// <summary>
        /// Gets the Dapper Extensions Implementation
        /// </summary>
        private static IDapperImplementor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = InstanceFactory(_configuration);
                        }
                    }
                }

                return _instance;
            }
        }

        static DapperHelper()
        {
            Configure(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect(), null);
        }

        /// <summary>
        /// Add other assemblies that Dapper Extensions will search if a mapping is not found in the same assembly of the POCO.
        /// </summary>
        /// <param name="assemblies"></param>
        public static void SetMappingAssemblies(IList<Assembly> assemblies)
        {
            Configure(_configuration.DefaultMapper, assemblies, _configuration.Dialect, _configuration.IdentityColumn);
        }

        /// <summary>
        /// Configure DapperExtensions extension methods.
        /// </summary>
        /// <param name="defaultMapper"></param>
        /// <param name="mappingAssemblies"></param>
        /// <param name="sqlDialect"></param>
        public static void Configure(IDapperExtensionsConfiguration configuration)
        {
            _instance = null;
            _configuration = configuration;
        }

        /// <summary>
        /// Configure DapperExtensions extension methods.
        /// </summary>
        /// <param name="defaultMapper"></param>
        /// <param name="mappingAssemblies"></param>
        /// <param name="sqlDialect"></param>
        public static void Configure(Type defaultMapper, IList<Assembly> mappingAssemblies, ISqlDialect sqlDialect, string identityColumn)
        {
            Configure(new DapperExtensionsConfiguration(defaultMapper, mappingAssemblies, sqlDialect, identityColumn));
        }

        /// <summary>
        /// Executes a query for the specified id, returning the data typed as per T
        /// </summary>
        public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.GetAsync<T>(connection, id, transaction, commandTimeout).Result;
        }

        public static async Task<T> GetAsync<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.GetAsync<T>(connection, id, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// </summary>
        public static IEnumerable<T> GetAll<T>(this IDbConnection connection, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetAllAsync<T>(connection, predicate, sort, transaction, commandTimeout, buffered).Result;
        }

        public static async Task<IEnumerable<T>> GetAllAsync<T>(this IDbConnection connection, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return await Instance.GetAllAsync<T>(connection, predicate, sort, transaction, commandTimeout, buffered);
        }

        public static async Task<IEnumerable<T>> GetAllAsync<T>(this IDbConnection connection, Expression<Func<T, bool>> predicate, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            var prGroup = PredicateConverter<T>.FromExpression(predicate);

            return await Instance.GetAllAsync<T>(connection, prGroup, sort, transaction, commandTimeout, buffered);
        }

        /// <summary>
        /// Executes an insert query for the specified entity.
        /// </summary>
        public static void Insert<T>(this IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Instance.Insert<T>(connection, entities, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes an insert query for the specified entity, returning the primary key.  
        /// If the entity has a single key, just the value is returned.  
        /// If the entity has a composite key, an IDictionary&lt;string, object&gt; is returned with the key values.
        /// The key value for the entity will also be updated if the KeyType is a Guid or Identity.
        /// </summary>
        public static dynamic Insert<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.InsertAsync<T>(connection, entity, transaction, commandTimeout).Result;
        }

        public static async Task<dynamic> InsertAcync<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.InsertAsync<T>(connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes an update query for the specified entity.
        /// </summary>
        public static bool Update<T>(this IDbConnection connection, T entity, IPredicate predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.UpdateAsync<T>(connection, entity, predicate, transaction, commandTimeout).Result;
        }

        public static async Task<bool> UpdateAcync<T>(this IDbConnection connection, T entity, IPredicate predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.UpdateAsync<T>(connection, entity, predicate, transaction, commandTimeout);
        }

        public static async Task<bool> UpdateAcync<T>(this IDbConnection connection, T entity, Expression<Func<T, bool>> predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var prGroup = PredicateConverter<T>.FromExpression(predicate);

            return await Instance.UpdateAsync<T>(connection, entity, prGroup, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a delete query for the specified entity.
        /// </summary>
        public static bool Delete<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Delete<T>(connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a delete query using the specified predicate.
        /// </summary>
        public static bool Delete<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Delete<T>(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified page and resultsPerPage.
        /// </summary>
        public static IEnumerable<T> GetPage<T>(this IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetPage<T>(connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered);
        }

        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified firstResult and maxResults.
        /// </summary>
        public static IEnumerable<T> GetSet<T>(this IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetSet<T>(connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);
        }

        /// <summary>
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        public static int Count<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Count<T>(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a select query for multiple objects, returning IMultipleResultReader for each predicate.
        /// </summary>
        public static IMultipleResultReader GetMultiple(this IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Instance.GetMultiple(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// Gets the appropriate mapper for the specified type T. 
        /// If the mapper for the type is not yet created, a new mapper is generated from the mapper type specifed by DefaultMapper.
        /// </summary>
        public static IClassMapper GetMap<T>() where T : class
        {
            return Instance.SqlGenerator.Configuration.GetMap<T>();
        }

        /// <summary>
        /// Clears the ClassMappers for each type.
        /// </summary>
        public static void ClearCache()
        {
            Instance.SqlGenerator.Configuration.ClearCache();
        }

        /// <summary>
        /// Generates a COMB Guid which solves the fragmented index issue.
        /// See: http://davybrion.com/blog/2009/05/using-the-guidcomb-identifier-strategy
        /// </summary>
        public static Guid GetNextGuid()
        {
            return Instance.SqlGenerator.Configuration.GetNextGuid();
        }
    }
}
