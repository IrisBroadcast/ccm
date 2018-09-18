using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IOwnersRepository : IRepository<Owner>
    {
        Owner GetByName(string name);
        List<Owner> FindOwners(string search);
    }
}