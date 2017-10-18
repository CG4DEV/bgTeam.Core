namespace Trcont.Booking.EntityDataBaseRsi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Images
    {
        public int ImageId { get; set; }

        public int? External { get; set; }

        public byte[] Content { get; set; }

        public string Path { get; set; }

        //public TimeSpan ValidState { get; set; }

        public Guid ImageGUID { get; set; }
    }
}
