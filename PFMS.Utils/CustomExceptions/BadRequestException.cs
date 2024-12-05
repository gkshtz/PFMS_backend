using System.Net;
using PFMS.Utils.Enums;

namespace PFMS.Utils.CustomExceptions
{
    public class BadRequestException: CustomException
    {
        public BadRequestException(): base((int)HttpStatusCode.BadRequest)
        {
            Name = ErrorNames.BAD_REQUEST_ERROR.ToString();
        }

        public BadRequestException(string message): base(message, (int)HttpStatusCode.BadRequest)
        {
            Name = ErrorNames.BAD_REQUEST_ERROR.ToString();
        }

    }
}
