namespace Trcont.Cud.Domain.Entity
{
    using DapperExtensions.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TableName("Point")]
    public class Station
    {
        [ColumnName("PointGuid")]
        public Guid StationGuid { get; set; }

        public int PointId { get; set; }

        public string Title { get; set; }

        public string ExternalCode { get; set; }

        public Guid CountryGuid { get; set; }
    }
}
