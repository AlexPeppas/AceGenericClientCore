

namespace Nbg.NetCore.Services.Ace.Http.Types
{
    public class AceClientRequest<T> : AceClientRequestHeaders
    {
        public T AceRequest { get; set; }
    }
}
