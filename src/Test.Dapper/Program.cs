using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Dapper;
using bgTeam.DataAccess.Impl.PostgreSQL;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Test.Dapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = new AppSettings();
            var dialect = new SqlDialectWithUnderscoresDapper();
            var factory = new ConnectionFactoryPostgreSQL(settings, dialect);
            var crud = new CrudServiceDapper(factory);

            var obj = new SendType()
            {
                Name = $"Test Dapper {Thread.CurrentThread.ManagedThreadId}",
            };

            crud.Insert(obj);
        }
    }

    public class AppSettings : IConnectionSetting
    {
        public string ConnectionString { get; set; }

        public AppSettings()
        {
            ConnectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=duktus_dev;";
        }
    }

    [TableName("send_types")]
    public class SendType
    {
        /// <summary>
        ///     Идентификатор сущности.
        /// </summary>
        [Identity]
        [ColumnName("id")]
        public long Id { get; set; }

        /// <summary>
        ///     Дата создания сущности.
        /// </summary>
        [ColumnName("create_date")]
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        ///     Дата обновления сущности.
        /// </summary>
        [ColumnName("update_date")]
        public DateTimeOffset UpdateDate { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [ColumnName("name")]
        [StringLength(256, ErrorMessage = "Maximum 256 characters")]
        public string Name { get; set; }
    }
}
