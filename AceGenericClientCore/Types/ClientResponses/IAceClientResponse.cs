using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class IAceClientResponse<R>
    {
        public R AceResponse { get; set; }
        public CbsErrorData AceError { get; set; }
    }
}
