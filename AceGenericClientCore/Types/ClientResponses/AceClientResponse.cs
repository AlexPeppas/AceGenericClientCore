

using System.Net;

namespace Nbg.NetCore.Services.Ace.Http.Types
{
    public class AceClientResponse<R>
    {
        public R AceResponse { get; set; }

        public CbsErrorData AceError { get; set; }

        public HttpStatusCode AceHttpStatusCode { get; set; }
    }
}
