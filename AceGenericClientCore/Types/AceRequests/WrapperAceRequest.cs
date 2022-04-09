
namespace Nbg.NetCore.Services.Ace.Http.Types
{
    internal class WrapperAceRequest<T>
    {
        public T Payload { get; set; }
        public RequestValidationControls ValidationControlsRequest { get; set; }
    }
}
