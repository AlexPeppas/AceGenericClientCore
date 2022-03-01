using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class IAceClientResponseWithControl<R> : IAceClientResponse<R>
    {
        public OutputMessage[] AceExceptionMessages { get; set; }
        public CbsInformationMessage[] AceInformationMessages { get; set; }
    }
}
