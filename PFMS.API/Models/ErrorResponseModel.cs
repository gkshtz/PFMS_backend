namespace PFMS.API.Models
{
    public class ErrorResponseModel
    {
        public int StatusCode { get; set; }
        public string ErrorName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
