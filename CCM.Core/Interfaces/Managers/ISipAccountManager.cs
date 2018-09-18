using System;
using System.Collections.Generic;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Managers
{
    public interface ISipAccountManager
    {
        SipAccount GetById(Guid userId);
        SipAccount GetByUserName(string userName);
        List<SipAccount> GetAll();
        List<SipAccount> Find(string startsWith);
        void Create(SipAccount account);
        void Update(SipAccount account);
        void UpdatePassword(Guid id, string password);
        void UpdateComment(Guid id, string comment);
        bool Delete(Guid id);
    }
}