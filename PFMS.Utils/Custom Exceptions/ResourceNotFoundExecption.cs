using PFMS.Utils.Enums;

namespace PFMS.Utils.Custom_Exceptions
{
    public class ResourceNotFoundExecption: CustomException
    {
        public ResourceNotFoundExecption(): base(404)
        {
            Name = ErrorNames.RESOURCE_NOT_FOUND_ERROR.ToString();
        }
        public ResourceNotFoundExecption(string message): base(message, 404)
        {
            Name = ErrorNames.RESOURCE_NOT_FOUND_ERROR.ToString();
        }
    }
}
