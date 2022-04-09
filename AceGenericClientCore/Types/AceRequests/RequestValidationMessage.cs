
namespace Nbg.NetCore.Services.Ace.Http.Types
{
    internal class RequestValidationMessage
    {
        public string ValidationCode { get; set; }
        public string ValidationType { get; set; } //Exception / Information
        public string ValidationDescription { get; set; }
        public string AuthUser { get; set; }
        public string AuthRole { get; set; }
        public string RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; }

    }
}
