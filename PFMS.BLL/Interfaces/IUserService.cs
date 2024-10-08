﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.BLL.BOs;

namespace PFMS.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserBo> AddUserAsync(UserBo userBo);

        public Task<string> AuthenticateUser(UserCredentialsBo userCredentialsBo);
    }
}
