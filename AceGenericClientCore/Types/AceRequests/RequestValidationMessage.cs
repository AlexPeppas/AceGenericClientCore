
namespace AceGenericClientFramework.Types
{
    internal class RequestValidationMessage
    {
        public string ValidationCode;
        public string ValidationType; //Exception / Information
        public string ValidationDescription;
        public string AuthUser;
        public string AuthRole;

    }
}
