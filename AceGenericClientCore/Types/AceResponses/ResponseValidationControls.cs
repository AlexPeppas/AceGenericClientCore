using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class ResponseValidationControls
    {
        public bool ExecuteTranIfValid { get; set; }
        public bool AllValidationAreFullfiled { get; set; }
        public List<ResponseValidationMessage> ValidationsRequired { get; set; }
    }
}
