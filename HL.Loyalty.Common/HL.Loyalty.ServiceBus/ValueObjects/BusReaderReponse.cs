using HL.Loyalty.ServiceBus.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.ServiceBus.ValueObjects
{
    public class BusReaderReponse
    {
        public BusReaderReponse()
        {
            Status = StatusResponse.Failure;
        }

        public StatusResponse Status { get; set; }
        public object Response { get; set; }
        public Exception Exception { get; set; }
    }
}
