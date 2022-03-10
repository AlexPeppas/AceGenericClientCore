

namespace AceGenericClientFramework.Types
{
    public class AceClientRequestHeaders
    {
        public string UserId { get; set; }

        public string GlobalUUID { get; set; }

        public string RequestUUID { get; set; }

        public Enums.AceClientLang Lang { get; set; }

        public string SecurityToken { get; set; }

        public string WorkstationId { get; set; }

        public string BranchId { get; set; }
    }
}
