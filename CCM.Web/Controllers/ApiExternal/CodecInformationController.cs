using System.Collections.Generic;
using System.Web.Http;
using CCM.Core.CodecControl.Entities;
using CCM.Core.Interfaces.Repositories;

namespace CCM.Web.Controllers.ApiExternal
{
    public class CodecInformationController : ApiController
    {
        private readonly IRegisteredSipRepository _registeredSipRepository;

        public CodecInformationController(IRegisteredSipRepository registeredSipRepository)
        {
            _registeredSipRepository = registeredSipRepository;
        }

        public IList<CodecInformation> Get()
        {
            var codecInformationList = _registeredSipRepository.GetCodecInformationList();
            return codecInformationList;
        }

        public CodecInformation Get(string sipAddress)
        {
            var codecInformation = _registeredSipRepository.GetCodecInformation(sipAddress);
            return codecInformation;
        }
    }
}