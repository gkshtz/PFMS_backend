using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PFMS.BLL.Interfaces;
using PFMS.DAL.Interfaces;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Constants;
using AutoMapper;
using PFMS.BLL.BOs;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using PFMS.DAL.DTOs;

namespace PFMS.BLL.Services
{
    public class OneTimePasswordsService: IOneTimePasswordsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOneTimePasswordsRespository _otpRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OneTimePasswordsService(IUserRepository userRepository, IMapper mapper, IOneTimePasswordsRespository otpRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task VerifyOtp(string otp, string email)
        {
            var userDto = await _userRepository.FindUserByEmail(email);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var userBo = _mapper.Map<UserBo>(userDto);

            var otpDto = await _otpRepository.FetchByOtp(otp);

            if(otpDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.OtpDoesNotFound);
            }

            var uniqueDeviceId = _httpContextAccessor.HttpContext.Request.Cookies[ApplicationConstsants.UniqueDeviceId];

            if(String.IsNullOrEmpty(uniqueDeviceId))
            {
                throw new BadRequestException(ErrorMessages.UniqueDeviceIdNotPresentInCookies);
            }

            var otpBo = _mapper.Map<OneTimePasswordBo>(otpDto);

            if(otpBo.UniqueDeviceId != Guid.Parse(uniqueDeviceId))
            {
                throw new ForbiddenException(ErrorMessages.UniqueDeviceIdNotMatch);
            }

            if(otpBo.UserId != userBo.UserId)
            {
                throw new ForbiddenException(ErrorMessages.UserNotAllowedToResetPassword);
            }

            if(otpBo.Expires < DateTime.UtcNow)
            {
                throw new BadRequestException(ErrorMessages.OtpIsExpired);
            }

            otpBo.IsVerified = true;

            otpDto = _mapper.Map<OneTimePasswordDto>(otpBo);

            await _otpRepository.UpdateOtp(otpDto);
        }
    }
}
