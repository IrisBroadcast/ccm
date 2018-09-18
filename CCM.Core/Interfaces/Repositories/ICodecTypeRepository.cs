using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ICodecTypeRepository : IRepository<CodecType>
    {
        List<CodecType> Find(string search, bool includeUsers = true);
        List<CodecType> GetAll(bool includeUsers = true);
    }
}