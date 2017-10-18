namespace Trcont.CitTrans.Service.Entity
{
    using System;
    using Trcont.CitTrans.Service.CitTrans;

    public class DislocationInfo
    {
        public string ContNumber { get; set; } //KontNumber

        public DateTime? Date { get; set; } //OperDate

        public Station Station { get; set; } //OperStation

        public Value Operation { get; set; } //Operation

        public DislocationInfo() { }

        public DislocationInfo(Container container)
        {
            ContNumber = container.ContainerNumber;

            if (container.LastOperation != null)
            {
                Date = container.LastOperation.ServiceDate;
                Operation = new Value()
                {
                    Code = container.LastOperation.ServiceCode,
                    Name = container.LastOperation.ServiceName
                };

                if (container.LastOperation.Station != null)
                {
                    var station = container.LastOperation.Station;
                    Station = new Station()
                    {
                        StationCode = station.StationCode,
                        StationName = station.StationName
                    };

                    if (Station.Country != null)
                    {
                        var country = station.Country;
                        Station.Country = new Value()
                        {
                            Code = country.Code.ToString(),
                            Name = country.Name
                        };
                    }
                }
            }
        }
    }
}
