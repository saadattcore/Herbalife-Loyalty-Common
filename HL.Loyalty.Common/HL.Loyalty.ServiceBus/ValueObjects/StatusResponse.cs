using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.ServiceBus.ValueObjects
{
    public enum StatusResponse
    {
        Success=0,
        Failure=1,
        Timeout=2,
        Missingconfiguration=3        
    }
}
