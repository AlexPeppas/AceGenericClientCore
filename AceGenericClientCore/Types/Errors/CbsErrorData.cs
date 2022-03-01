using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public class CbsErrorData
    {
        public errorResponse ErrorResponse { get; set; }

    }

    public class errorResponse
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string System { get; set; }

        public string Type { get; set; }

        public SystemErrorResponse[] SystemErrorResponse { get; set; }
    }
}
