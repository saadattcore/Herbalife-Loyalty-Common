using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.ServiceBus.Interfaces
{
    public interface IBusPublisher
    {        
        void Publish<T>(T value, string MessageType = null);
    }
}
