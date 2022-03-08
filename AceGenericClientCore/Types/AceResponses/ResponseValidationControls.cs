using System.Collections.Generic;

namespace AceGenericClientFramework.Types
{
    internal class ResponseValidationControls
    {
        public bool ExecuteTranIfValid { get; set; }
        public bool AllValidationAreFullfiled { get; set; }
        public List<ResponseValidationMessage> ValidationsRequired { get; set; }
    }
}
