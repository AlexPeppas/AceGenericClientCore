

using System.Collections.Generic;

namespace Nbg.NetCore.Services.Ace.Http.Types
{
    public class OutputMessage
    {
        public string Code { get; set; }

        public string[] ActionGroups { get; set; }

        public string Description { get; set; }

        public string RelatedEntityId { get; set; }

        public string RelatedEntityType { get; set; }

        public string SourceSystemCode { get; set; }
    }
}
