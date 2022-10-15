using HL.Loyalty.ServiceBus.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.ServiceBus.Interfaces
{
    public interface IBusReader
    {
        BusReaderReponse getMessageByID<T>(string sessionId);        

        BusReaderReponse getMessage<T>();
    }
}
