using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    }
}
