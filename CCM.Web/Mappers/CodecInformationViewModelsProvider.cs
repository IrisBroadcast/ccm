using System;
using System.Collections.Generic;
using System.Linq;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Models.ApiExternal;

namespace CCM.Web.Mappers
{
    public class CodecInformationViewModelsProvider
    {
        private readonly IRegisteredSipRepository _registeredSipRepository;

        public CodecInformationViewModelsProvider(IRegisteredSipRepository registeredSipRepository)
        {
            _registeredSipRepository = registeredSipRepository;
        }

        public IEnumerable<CodecInformationViewModel> GetAll()
        {
            var registeredUserAgents = _registeredSipRepository.GetRegisteredUserAgentsCodecInformation();

            return registeredUserAgents.Select(x =>
            {
                return new CodecInformationViewModel(
                    sipAddress: x.SipAddress,
                    ip: x.Ip,
                    api: x.Api,
                    gpoNames: x.GpoNames,
                    nrOfInputs: x.NrOfInputs,
                    nrOfGpos: x.NrOfGpos);
            }).ToList();

        }
    }
}