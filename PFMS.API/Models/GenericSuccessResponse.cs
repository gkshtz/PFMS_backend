using PFMS.Utils.Enums;

namespace PFMS.API.Models
{
    public class GenericSuccessResponse<T>
    {
        public int StatusCode { get; set; }
        public T? ResponseData { get; set; }
        public string ResponseMessage { get; set; }
    }
}
