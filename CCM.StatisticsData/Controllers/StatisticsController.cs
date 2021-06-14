using CCM.StatisticsData.Interfaces;
using CCM.StatisticsData.Repositories;
using CCM.StatisticsData.Statistics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CCM.StatisticsData.Statistics.LocationStatisticsOverview;

namespace CCM.StatisticsData.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ISipAccountRepository _sipAccountRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICodecTypeRepository _codecTypeRepository;

        public StatisticsController(IStatisticsRepository statisticsRepository,
            IRegionRepository regionsRepository,
            ISipAccountRepository sipAccountRepository,
            IOwnerRepository ownerRepository,
            ICodecTypeRepository codecTypeRepository)
        {
            _statisticsRepository = statisticsRepository;
            _regionRepository = regionsRepository;
            _sipAccountRepository = sipAccountRepository;
            _ownerRepository = ownerRepository;
            _codecTypeRepository = codecTypeRepository;
        }


        //[HttpGet]
        //public IActionResult GetLocationStatistics(DateTime startDate, DateTime endDate, Guid regionId, Guid ownerId, Guid codecTypeId)
        
        //{
        //    return Ok(_statisticsRepository.GetLocationStatistics(startDate.ToUniversalTime(), endDate.ToUniversalTime(), regionId, ownerId, codecTypeId));
        //}

        //[HttpGet]
        [Route("Api/Statistics/GetRegions")]
        public IActionResult GetRegions()
        {
            return Ok(_regionRepository.GetAll());
        }

        [Route("Api/Statistics/GetCodecTypes")]
        public IActionResult GetCodecTypes()
        {
            return Ok(_codecTypeRepository.GetAll());
        }

        [Route("Api/Statistics/GetOwners")]
        public IActionResult GetOwners()
        {
            return Ok(_ownerRepository.GetAll());
        }

        [Route("Api/Statistics/GetSipAccounts")]
        public IActionResult GetSipAccounts()
        {
            return Ok(_sipAccountRepository.GetAll());
        }

       // [Route("Api/Statistics/GetLocationBasedStatistics")]
        //public IActionResult GetLocationBasedStatistics(Guid regionId, Guid ownerId, Guid codecTypeId, DateTime startTime, DateTime endTime)
        //{
        //    var statistics = _statisticsRepository.GetLocationStatistics(startTime, endTime, regionId, ownerId, codecTypeId);
        //    return Ok(statistics);
        //}

       // [Route("Api/Statistics/GetLocationNumberOfCallsTable")]
        //public IActionResult GetLocationNumberOfCallsTable(DateTime startTime, DateTime endTime, Guid regionId, Guid ownerId, Guid codecTypeId)
        //{
        //    var locationStats = new LocationStatisticsOverview
        //    {
        //        Mode = LocationStatisticsMode.NumberOfCalls,
        //        StartDate = startTime,
        //        EndDate = endTime,
        //        RegionId = regionId,
        //        OwnerId = ownerId,
        //        CodecTypeId = codecTypeId,
        //        Statistics = _statisticsRepository.GetLocationStatistics(startTime.ToUniversalTime(), endTime.ToUniversalTime().AddDays(1.0), regionId, ownerId, codecTypeId)
        //    };
        //    return Ok(locationStats);
        //}
        [Route("Api/Statistics/GetLocationNumberOfCallsTable")]
        public IActionResult GetLocationNumberOfCallsTable(DateTime startTime, DateTime endTime, Guid regionId, Guid ownerId)
        {
            var locationStats = new LocationStatisticsOverview
            {
                Mode = LocationStatisticsMode.NumberOfCalls,
                StartDate = startTime,
                EndDate = endTime,
                RegionId = regionId,
                OwnerId = ownerId,
                Statistics = _statisticsRepository.GetLocationStatistics(startTime.ToUniversalTime(), endTime.ToUniversalTime().AddDays(1.0), regionId, ownerId)
            };
            return Ok(locationStats);
        }

        //public IActionResult GetLocationsNumberOfCallsTableWithCodecType(DateTime startTime, DateTime endtime, Guid regionId, Guid ownerId)
        //{

        //    return Ok()
        //}

        [Route("Api/Statistics/GetCodecTypeStatistics")]
        public IActionResult GetCodecTypeStatistics(DateTime startTime, DateTime endTime, Guid codecTypeId)
        {
             var statistics = _statisticsRepository.GetCodecTypeStatistics(startTime.ToUniversalTime(), endTime.ToUniversalTime(), codecTypeId);
             return Ok(statistics);
        }

        [Route("Api/Statistics/GetRegionStatistics")]
        public IActionResult GetRegionStatistics(Guid regionId, DateTime startTime, DateTime endTime)
        {
            var statistics = _statisticsRepository.GetRegionStatistics(startTime.ToUniversalTime(), endTime.ToUniversalTime(), regionId);
            return Ok(statistics);
        }
        [Route("Api/Statistics/GetSipStatistics")]
        public IActionResult GetSipStatistics(Guid sipId, DateTime startTime, DateTime endTime)
        {
            var statistics = _statisticsRepository.GetSipStatistics(startTime.ToUniversalTime(), endTime.ToUniversalTime(), sipId);
            return Ok(statistics);
        }
        [Route("Api/Statistics/GetCategoryStatistics")]
        public IActionResult GetCategoryStatistics(DateTime startTime, DateTime endTime)
        {
            var statistics = _statisticsRepository.GetCategoryStatistics(startTime.ToUniversalTime(), endTime.ToUniversalTime());
            return Ok(statistics);
        }
    }
}
