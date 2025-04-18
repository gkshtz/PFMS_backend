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
        public Task<List<UserBo>> GetAllUsers();
        public Task<UserBo> AddUserAsync(UserBo userBo);

        public Task<TokenBo> AuthenticateUser(UserCredentialsBo userCredentialsBo);

        public Task UpdateUserProfile(UserBo userBo, Guid userId);

        public Task UpdatePassword(string oldPassword, string newPassword, Guid userId);

        public Task<UserBo> GetUserProfile(Guid userId);

        public Task<string> RefreshAccessToken();

        public void Logout();

        public Task DeleteUserAsync(Guid userId);
    }
}
