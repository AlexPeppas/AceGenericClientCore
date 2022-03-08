

namespace AceGenericClientFramework.Types
{
    public class AceClientRequestWithControl<T> : AceClientRequest<T>
    {
        public bool CanBeExecuted { get; set; }
        public InputMessage[] AceMessages { get; set; }
    }
}
