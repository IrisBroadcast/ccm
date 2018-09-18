using System;
using System.Collections.Generic;
using System.Linq;
using CCM.Core.Attributes;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Core.Managers
{
    /// <summary>
    /// Handles the filter properties for RegisteredSip.
    /// </summary>
    public class FilterManager : IFilterManager
    {
        private readonly IFilterRepository _filterRepository;

        public FilterManager(IFilterRepository filterRepository)
        {
            _filterRepository = filterRepository;
        }

        /// <summary>
        /// Hämtar lista med egenskaper hos CachedRegisteredSip-objektet som är möjliga att filtrera på.
        /// </summary>
        /// <returns></returns>
        public List<AvailableFilter> GetFilterProperties()
        {
            var filterProperties = (typeof(RegisteredSipDto)).GetProperties().Where(p => p.GetCustomAttributes(false).Any(a => a is FilterPropertyAttribute));

            List<AvailableFilter> availableFilters = new List<AvailableFilter>();

            foreach (var property in filterProperties)
            {
                var filterAttribute = property.GetCustomAttributes(typeof(FilterPropertyAttribute), false).FirstOrDefault() as FilterPropertyAttribute;

                if (filterAttribute != null)
                {
                    var filter = new AvailableFilter
                    {
                        FilteringName = property.Name,
                        TableName = filterAttribute.TableName,
                        ColumnName = filterAttribute.ColumnName
                    };
                    availableFilters.Add(filter);
                }
                
            }

            return availableFilters;
        }

        /// <summary>
        /// Checks the filter name availability.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool CheckFilterNameAvailability(string name, Guid id)
        {
            return _filterRepository.CheckFilterNameAvailability(name, id);
        }

        public void Save(Filter filter)
        {
            _filterRepository.Save(filter);
        }

        public List<Filter> GetAllFilters()
        {
            return _filterRepository.GetAll() ?? new List<Filter>();
        }

        /// <summary>
        /// Hämtar aktiva filter inklusive lista med deras möjliga val
        /// </summary>
        /// <returns></returns>
        public List<AvailableFilter> GetAvailableFiltersIncludingOptions()
        {
            var filters = GetAllFilters() ?? new List<Filter>();

            return filters.Select(filter => new AvailableFilter
            {
                Name = filter.Name,
                FilteringName = filter.FilteringName,
                TableName = filter.TableName,
                ColumnName = filter.ColumnName,
                Options = _filterRepository.GetFilterPropertyValues(filter.TableName, filter.ColumnName)
            }).ToList();
        }

        public Filter GetFilter(Guid id)
        {
            return _filterRepository.GetById(id);
        }

        public void Delete(Guid id)
        {
            _filterRepository.Delete(id);
        }

        public List<Filter> FindFilters(string search)
        {
            return _filterRepository.Find(search);
        }
    }
}