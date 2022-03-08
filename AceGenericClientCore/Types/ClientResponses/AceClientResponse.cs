

namespace AceGenericClientFramework.Types
{
    public class AceClientResponse<R>
    {
        public R AceResponse { get; set; }
        public CbsErrorData AceError { get; set; }
    }
}
