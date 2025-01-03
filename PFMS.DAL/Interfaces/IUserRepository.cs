using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<UserDto>> GetAllUsers();
        public Task<UserDto> AddUser(UserDto userDto, TotalTransactionAmountDto totalTransactionAmountDto);

        public Task<UserDto> FindUserByEmail(string email);

        public Task<UserDto> GetUserById(Guid userId);

        public Task<bool> UpdateUser(UserDto userDto);

        public Task<bool> UpdatePassword(string password, Guid userId);

        public Task<UserDto> GetUserProfile(Guid userId);
    }
}
