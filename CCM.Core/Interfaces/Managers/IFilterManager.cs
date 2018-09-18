using System;
using System.Collections.Generic;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Managers
{
    public interface IFilterManager
    {
        List<AvailableFilter> GetFilterProperties();
        bool CheckFilterNameAvailability(string name, Guid id);
        void Save(Filter filter);
        List<Filter> GetAllFilters();
        List<AvailableFilter> GetAvailableFiltersIncludingOptions();
        Filter GetFilter(Guid id);
        void Delete(Guid id);
        List<Filter> FindFilters(string search);
    }
}
