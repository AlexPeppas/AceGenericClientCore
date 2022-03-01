using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class RequestValidationMessage
    {
        public string ValidationCode;
        public string ValidationType; //Exception / Information
        public string ValidationDescription;
        public string AuthUser;
        public string AuthRole;

    }
}
