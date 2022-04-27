
namespace Nbg.NetCore.Services.Ace.Http.Types
{
    internal class ResponseValidationMessage
    {
        public string ValidationCode { get; set; }

        public string ValidationType { get; set; } //exception , information

        public string ValidationDescription { get; set; }

        public string MinAuthLevel { get; set; }

        public bool ValidationIsFullfiled { get; set; }

        public string RelatedEntityId { get; set; }

        public string RelatedEntityType { get; set; }
    }
}
