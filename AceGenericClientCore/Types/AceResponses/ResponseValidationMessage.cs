
namespace AceGenericClientFramework.Types
{
    internal class ResponseValidationMessage
    {
        public string ValidationCode { get; set; }
        public string ValidationType { get; set; } //exception , information
        public string ValidationDescription { get; set; }
        public string MinAuthLevel { get; set; }
        public bool ValidationIsFullfiled { get; set; }
    }
}
