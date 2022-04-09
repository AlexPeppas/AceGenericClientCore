

namespace Nbg.NetCore.Services.Ace.Http.Types
{
    public class AceClientResponseWithControl<R> : AceClientResponse<R>
    {
        public OutputMessage[] AceExceptionMessages { get; set; }
        public CbsInformationMessage[] AceInformationMessages { get; set; }
    }
}
