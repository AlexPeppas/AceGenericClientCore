

namespace AceGenericClientFramework.Types
{
    public class AceClientRequest<T> : AceClientRequestHeaders
    {
        public T AceRequest { get; set; }
    }
}
