using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ICcmUserRepository
    {
        CcmUser GetById(Guid userId);
        List<CcmUser> GetAll();
        CcmUser GetByUserName(string userName);
        List<CcmUser> FindUsers(string startsWith);
        bool Create(CcmUser ccmUser);
        bool Update(CcmUser ccmUser);
        void UpdatePassword(Guid id, string passwordHash, string salt);
        bool Delete(Guid userId);
        CcmRole GetUserRole(CcmUser ccmUser);
        Task<bool> AuthenticateAsync(string username, string password);
        void ImportLocalPasswords();
        void ImportCcmUserPasswords(List<UserInfo> userInfoList);
    }
}