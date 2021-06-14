using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsData.Repositories
{
    public class CodecTypeRepository : ICodecTypeRepository
    {
        private readonly StatsDbContext _statsDbContext;
        public CodecTypeRepository(StatsDbContext statsDbContext)
        {
            _statsDbContext = statsDbContext;
        }

        public StatsDbContext StatsDbContext { get; }

        public List<CodecTypeEntity> GetAll()
        {
            return _statsDbContext.CodecTypes?
               .AsEnumerable()
               .OrderBy(c => c.Name)
               .ToList();
        }
    }
}
