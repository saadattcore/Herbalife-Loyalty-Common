using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public enum ServiceResponseStateType
    {
        Unknown = 0,
        Succeeded = 1,
        FailedWithAFatalException = 2,
        FailedWithAnUnexpectedException = 3,
        FailedAuthentication = 11,
        FailedAuthorisation = 12,
        FailedValidation = 21
    }
}
