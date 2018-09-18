using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using CCM.Core.Helpers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Models.ApiExternal;

namespace CCM.Web.Controllers.ApiExternal
{
    /// <summary>
    /// Returnerar lista med alla registrerade kodare och deras sip-address, display name, och boolean som visar om kodaren är i samtal.
    /// Behov från OB-väskan.
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CodecsOnlineController : ApiController
    {
        private readonly IRegisteredSipRepository _registeredSipRepository;

        public CodecsOnlineController(IRegisteredSipRepository registeredSipRepository)
        {
            _registeredSipRepository = registeredSipRepository;
        }

        public IList<CodecInfo> Get()
        {
            using (new TimeMeasurer("GetCachedRegisteredSips CodecsOnlineController"))
            {
                var registeredSipsInfo = _registeredSipRepository.GetCachedRegisteredSips().Select(rs =>
                    new CodecInfo()
                    {
                        SipAddress = rs.Sip,
                        DisplayName = rs.DisplayName,
                        InCall = rs.InCall
                    }).ToList();
                return registeredSipsInfo;
            }
        }
    }
}