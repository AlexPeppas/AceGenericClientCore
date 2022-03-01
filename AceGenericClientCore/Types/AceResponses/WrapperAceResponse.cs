using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class WrapperAceResponse<R>
    {
        public R Payload { get; set; }
        public ResponseValidationControls ValidationControlsRespo { get; set; }
    }
}
