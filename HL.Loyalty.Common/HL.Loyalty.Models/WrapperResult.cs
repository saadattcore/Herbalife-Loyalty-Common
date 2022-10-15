using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL.Loyalty.Models
{
    public class WrapperResult<T>
    {
        public T DataResult { get; set; }
        public Exception ErrorResult { get; set; }
        public WrapperResultType Status { get; set; }
        public string ErrorMessage { get; set; }

    }
}
