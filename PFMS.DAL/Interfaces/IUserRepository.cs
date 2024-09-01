using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;

namespace PFMS.DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserDto> AddUser(UserDto userDto, TotalTransactionAmountDto totalTransactionAmountDto);
    }
}
