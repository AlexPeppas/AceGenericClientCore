using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class IAceClientRequestWithControl<T> : IAceClientRequest<T>
    {
        public bool CanBeExecuted { get; set; }
        public InputMessage[] AceMessages { get; set; }
    }
}
