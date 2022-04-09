using System.Collections.Generic;

namespace Nbg.NetCore.Services.Ace.Http.Types
{
    internal class RequestValidationControls
    {
        public bool ExecuteTranIfValid { get; set; }

        public List<RequestValidationMessage> ValidationsFullfiled { get; set; }
    }
}
