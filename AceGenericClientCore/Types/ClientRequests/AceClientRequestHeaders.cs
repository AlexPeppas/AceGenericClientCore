

using System.Collections.Generic;

namespace AceGenericClientFramework.Types
{
    public class AceClientRequestHeaders
    {
        public AceClientRequestHeaders()
        {
            Headers = new Dictionary<string, string>();
        }

        public Dictionary<string,string> Headers { get; set; }

        public string UserId { get; set; }

        public string GlobalUUID { get; set; }

        public string RequestUUID { get; set; }

        public Enums.AceClientLang Lang { get; set; }

        public string SecurityToken { get; set; }

        public string WorkstationId { get; set; }

        public string BranchId { get; set; }

        public string MockMode { get; set; }

        public string SandboxId { get; set; }

        public string UserName { get; set; }
    }
}
