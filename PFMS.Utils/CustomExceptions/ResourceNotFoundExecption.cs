using PFMS.Utils.Enums;

namespace PFMS.Utils.CustomExceptions
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
