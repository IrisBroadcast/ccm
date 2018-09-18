using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ISipAccountRepository
    {
        SipAccount GetById(Guid id);
        SipAccount GetByUserName(string userName);
        List<SipAccount> GetAllIncludingRelations();
        List<SipAccount> GetAll();
        List<SipAccount> Find(string startsWith);
        void Create(SipAccount ccmUser);
        void Update(SipAccount ccmUser);
        void UpdateComment(Guid id, string comment);
        void UpdatePassword(Guid id, string password);
        void Delete(Guid id);
        Task<bool> AuthenticateAsync(string username, string password);
        void ImportSipAccountPasswords(List<UserInfo> userInfoList);
        
    }
}