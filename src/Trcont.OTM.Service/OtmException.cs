namespace Trcont.OTM.Service
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Trcont.OTM.Service.WebService;

    public class OtmException : Exception
    {
        private TransmissionAck _transmissionAck;

        public OtmException(string message) : base(message)
        {

        }

        public OtmException(string message, TransmissionAck transmissionAck) : base(message)
        {
            _transmissionAck = transmissionAck;
        }

        public string TransmissionStackTrace => _transmissionAck?.StackTrace;

        public string TransmissionStatus => _transmissionAck?.TransmissionAckStatus;
    }
}
