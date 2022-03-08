using System.Collections.Generic;

namespace AceGenericClientFramework.Types
{
    internal class RequestValidationControls
    {
        public bool ExecuteTranIfValid { get; set; }

        public List<RequestValidationMessage> ValidationsFullfiled { get; set; }
    }
}
