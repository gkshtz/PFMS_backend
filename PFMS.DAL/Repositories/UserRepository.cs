using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public UserRepository(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }
        public async Task<UserDto> AddUser(UserDto userDto, TotalTransactionAmountDto totalTransactionAmountDto)
        {
            var user = _mapper.Map<User>(userDto);
            var totalTransactionAmounts = _mapper.Map<TotalTransactionAmount>(totalTransactionAmountDto);
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.TotalTransactionAmounts.AddAsync(totalTransactionAmounts);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> FindUserByEmail(string email)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x=>x.Email == email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserById(Guid userId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUser(UserDto userDto)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userDto.UserId);
            if(user == null)
            {
                return false;
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.City = userDto.City;
            user.Age = userDto.Age;

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePassword(string newPassword, Guid userId)
        {
            var userDto = await GetUserById(userId);
            if(userDto == null)
            {
                return false;
            }

            userDto.Password = newPassword;

            var user = _mapper.Map<User>(userDto);

            _appDbContext.Users.Update(user);

            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
