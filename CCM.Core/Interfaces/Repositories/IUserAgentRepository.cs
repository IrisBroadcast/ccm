using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IUserAgentRepository : IRepository<UserAgent>
    {
        List<UserAgent> Find(string search);
    }
}