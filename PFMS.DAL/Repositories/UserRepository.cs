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
        public UserRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _appDbContext.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> AddUser(UserDto userDto, TotalTransactionAmountDto totalTransactionAmountDto)
        {
            var user = _mapper.Map<User>(userDto);
            var totalTransactionAmounts = _mapper.Map<TotalTransactionAmount>(totalTransactionAmountDto);
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.TotalTransactionAmounts.AddAsync(totalTransactionAmounts);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> FindUserByEmail(string email)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x=>x.Email == email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserById(Guid userId)
        {
            var user = await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
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

            return true;
        }

        public async Task<UserDto> GetUserProfile(Guid userId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            return _mapper.Map<UserDto>(user);
        }
    }
}
