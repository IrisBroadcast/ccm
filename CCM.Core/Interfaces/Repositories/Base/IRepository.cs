using System;
using System.Collections.Generic;
using CCM.Core.Entities.Base;

namespace CCM.Core.Interfaces.Repositories.Base
{
    public interface IRepository<T> where T : CoreEntityBase
    {
        void Save(T item);
        void Delete(Guid id);
        T GetById(Guid id);
        List<T> GetAll();
    }
}