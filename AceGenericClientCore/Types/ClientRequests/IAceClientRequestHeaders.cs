using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class IAceClientRequestHeaders
    {
        public string UserId { get; set; }

        public string GlobalUUID { get; set; }

        public string RequestUUID { get; set; }

        public Enums.AceClientLang Lang { get; set; }

        public string SecurityToken { get; set; }
    }
}
