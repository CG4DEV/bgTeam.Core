namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class vPoints : IIrsDictionary
    {
        public int Id { get; set; }

        public Guid IrsGuid { get; set; }

        public string Title { get; set; }

        public string Account { get; set; }

        public int CountryId { get; set; }

        public string Paragraphs { get; set; }

        public string CnsiCode { get; set; }

        public Guid? CnsiGuid { get; set; }

        public int PointType { get; set; }
    }
}
