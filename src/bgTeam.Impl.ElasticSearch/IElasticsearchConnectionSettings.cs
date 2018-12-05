namespace bgTeam.Impl.ElasticSearch
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Elasticsearch connection settings
    /// </summary>
    public interface IElasticsearchConnectionSettings
    {
        /// <summary>
        /// Elastic search nodes
        /// </summary>
        IEnumerable<string> Nodes { get; set; }
    }
}
