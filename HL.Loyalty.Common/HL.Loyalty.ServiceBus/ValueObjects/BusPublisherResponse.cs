using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.ServiceBus.ValueObjects
{
    public class BusPublisherResponse
    {
        public BusPublisherResponse()
        {
            Status = StatusResponse.Failure;
        }
        public StatusResponse Status { get; set; }
        public string sessionID{ get; set; }
        public object Response { get; set; }
    }
}
