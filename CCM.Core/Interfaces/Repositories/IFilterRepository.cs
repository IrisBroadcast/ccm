﻿using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IFilterRepository : IRepository<Filter>
    {
        List<string> GetFilterPropertyValues(string filterType, string filterProperty);
        bool CheckFilterNameAvailability(string name, Guid id);
        List<Filter> Find(string search);
    }
}
