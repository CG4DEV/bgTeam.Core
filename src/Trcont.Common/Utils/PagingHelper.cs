namespace Trcont.Common.Utils
{
    using System;

    public static class PagingHelper
    {
        public static int GetOffsetByPageNumber(int pageNumber, int pageSize)
        {
            if (pageSize < 0)
            {
                throw new ArgumentException("Размер страницы не может быть отрицательным");
            }

            if (pageNumber <= 0)
            {
                return 0;
            }

            return (pageNumber - 1) * pageSize;
        }
    }
}
