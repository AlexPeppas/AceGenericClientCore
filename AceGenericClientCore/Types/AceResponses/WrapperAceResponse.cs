
namespace AceGenericClientFramework.Types
{
    internal class WrapperAceResponse<R>
    {
        public R Payload { get; set; }
        public ResponseValidationControls ValidationControlsRespo { get; set; }
    }
}
