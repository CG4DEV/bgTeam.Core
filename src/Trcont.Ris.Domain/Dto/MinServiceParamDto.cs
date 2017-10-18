namespace Trcont.Ris.Domain.Dto
{
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Entity;

    public class MinServiceParamDto : ServiceParam
    {
        public string DictionaryName { get; set; }

        public IEnumerable<ServiceParamValues> Values { get; set; }
    }
}
