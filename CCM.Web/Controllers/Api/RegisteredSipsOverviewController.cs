using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Mappers;
using CCM.Web.Models.Home;

namespace CCM.Web.Controllers.Api
{
    public class RegisteredSipsOverviewController : ApiController
    {
        #region Constructor and members

        private readonly ISettingsManager _settingsManager;
        private readonly IRegisteredSipRepository _registeredSipRepository;

        public RegisteredSipsOverviewController(IRegisteredSipRepository registeredSipRepository, ISettingsManager settingsManager)
        {
            _registeredSipRepository = registeredSipRepository;
            _settingsManager = settingsManager;
        }
        #endregion

        public IList<RegisteredSipOverviewDto> Post()
        {
            var sipDomain = _settingsManager.SipDomain;
            var registeredSips = _registeredSipRepository.GetCachedRegisteredSips();
            var dtos = registeredSips.Select(sip => RegisteredSipOverviewDtoMapper.MapToDto(sip, sipDomain)).ToList();
            return dtos;
        }

    }
}
