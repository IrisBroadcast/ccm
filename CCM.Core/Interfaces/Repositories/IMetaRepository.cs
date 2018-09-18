using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IMetaRepository : IRepository<MetaType>
    {
        List<string> GetMetaTypePropertyValues(AvailableMetaType metaType);
        bool CheckMetaTypeNameAvailability(string name, Guid id);
        List<AvailableMetaType> GetMetaTypeProperties();
        List<MetaType> FindMetaTypes(string search);
    }
}