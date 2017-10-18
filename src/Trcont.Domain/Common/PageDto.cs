namespace Trcont.Domain.Common
{
    using System.Collections.Generic;

    public class PageDto<T>
        where T : class, new()
    {
        public IEnumerable<T> Page { get; set; }

        public int MatchCount { get; set; }

        //[JsonIgnore] // TODO : Удалить JsonIgnore, если понадобится общее количество заказов.
		// TODO : Нужно как не актуальное
        public int TotalCount { get; set; }
    }
}
