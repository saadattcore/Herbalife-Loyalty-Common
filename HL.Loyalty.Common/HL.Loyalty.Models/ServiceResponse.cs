using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class ServiceResponse
    {
        public Guid CorrelationId { get; set; }

        public ServiceResponseStateType State { get; set; }
    }
}
