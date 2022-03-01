using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class IAceClientRequest<T> : IAceClientRequestHeaders
    {
        public T AceRequest { get; set; }
    }
}
