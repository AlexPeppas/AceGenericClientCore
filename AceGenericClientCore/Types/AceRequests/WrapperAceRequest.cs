using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class WrapperAceRequest<T>
    {
        public T Payload { get; set; }
        public RequestValidationControls ValidationControlsRequest { get; set; }
    }
}
