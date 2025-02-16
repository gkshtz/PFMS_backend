using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class UserRepository<Dto, Entity>: GenericRepository<Dto, Entity>, IUserRepository<Dto>
        where Dto: UserDto
        where Entity: User
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public UserRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }


        public async Task<UserDto> FindUserByEmail(string email)
        {
            User? user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUser(UserDto userDto)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == userDto.Id);
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
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return false;
            }

            user.Password = newPassword;// We just update the entity and EF CORE will track the changes, we will save the changes later

            return true;
        }
    }
}
