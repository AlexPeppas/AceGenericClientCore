using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class RequestValidationControls
    {
        public bool ExecuteTranIfValid { get; set; }

        public List<RequestValidationMessage> ValidationsFullfiled { get; set; }
    }
}
