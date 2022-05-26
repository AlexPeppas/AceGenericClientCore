

using System.Collections.Generic;

namespace Nbg.NetCore.Services.Ace.Http.Types
{
    public class InputMessage
    {
        public string Code { get; set; }

        public List<string> ActionGroups { get; set; }

        public string AuthUser { get; set; }

        public List<string> AuthUserActionGroups { get; set; }

        public string RelatedEntityId { get; set; }

        public string RelatedEntityType { get; set; }

        public string SourceSystemCode { get; set; }
    }
}
