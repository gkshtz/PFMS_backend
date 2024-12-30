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
using Microsoft.AspNetCore.Identity;

namespace PFMS.BLL.Services
{
    public class OneTimePasswordsService: IOneTimePasswordsService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOneTimePasswordsRespository _otpRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher<UserBo> _passwordHasher;
        private readonly IEmailService _emailService;
        public OneTimePasswordsService(IUserRepository userRepository, IMapper mapper, IOneTimePasswordsRespository otpRepository, IHttpContextAccessor httpContextAccessor, IPasswordHasher<UserBo> passwordHasher, IEmailService emailService)
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<Guid> GenerateAndSendOtp(string email)
        {
            var userDto = await _userRepository.FindUserByEmail(email);

            if (userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var userBo = _mapper.Map<UserBo>(userDto);

            var otp = new Random().Next(100000, 999999).ToString();

            var deviceId = Guid.NewGuid();

            var otpBo = new OneTimePasswordBo()
            {
                OtpId = Guid.NewGuid(),
                Otp = otp,
                UserId = userBo.UserId,
                IsVerified = false,
                Expires = DateTime.UtcNow.AddMinutes(7),
                UniqueDeviceId = deviceId
            };

            var otpDto = _mapper.Map<OneTimePasswordDto>(otpBo);

            await _otpRepository.AddOtp(otpDto);

            string emailSubject = ApplicationConstsants.OtpEmailSubject;
            string emailBody = ApplicationConstsants.GenerateOtpEmailBody(otp, userBo.FirstName, 7);

            await _emailService.SendEmail(email, emailSubject, emailBody);

            return deviceId;
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

            //If OTP is already verified then we don't want to verify it again and for security reasons we will make it expired.
            if(otpBo.IsVerified)
            {
                otpBo.Expires = DateTime.UtcNow;
                otpDto = _mapper.Map<OneTimePasswordDto>(otpBo);

                await _otpRepository.UpdateOtp(otpDto);

                throw new BadRequestException(ErrorMessages.OtpAlreadyVerified);
            }

            otpBo.IsVerified = true;
            otpBo.Expires = DateTime.UtcNow.AddMinutes(3); // Now the password can be updated for next three minutes only

            otpDto = _mapper.Map<OneTimePasswordDto>(otpBo);

            await _otpRepository.UpdateOtp(otpDto);
        }

        public async Task ResetPassword(string password)
        {
            var uniqueDeviceId = _httpContextAccessor.HttpContext.Request.Cookies[ApplicationConstsants.UniqueDeviceId];
            if(String.IsNullOrEmpty(uniqueDeviceId))
            {
                throw new BadRequestException(ErrorMessages.UniqueDeviceIdNotPresentInCookies);
            }

            var otpDto = await _otpRepository.FetchByUniqueDeviceId(Guid.Parse(uniqueDeviceId));
            if(otpDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.OtpDoesNotFound);
            }

            var otpBo = _mapper.Map<OneTimePasswordBo>(otpDto);
            if(!otpBo.IsVerified)
            {
                throw new BadRequestException(ErrorMessages.OtpNotVerified);   
            }

            if(otpBo.Expires <= DateTime.UtcNow)
            {
                throw new BadRequestException(ErrorMessages.OtpIsExpired);
            }

            string newHashedPassword = _passwordHasher.HashPassword(null, password);

            await _userRepository.UpdatePassword(newHashedPassword, otpBo.UserId);

            otpBo.Expires = DateTime.UtcNow.AddHours(-1);
            otpDto = _mapper.Map<OneTimePasswordDto>(otpBo);
            await _otpRepository.UpdateOtp(otpDto);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(ApplicationConstsants.UniqueDeviceId, uniqueDeviceId, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/users/otp",
                Expires = DateTime.UtcNow.AddHours(-1)
            });
        }
    }
}
