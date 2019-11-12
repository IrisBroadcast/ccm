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
                // TODO: This one gives not right information, Headphones, Outputs and GPI's
                return new CodecInformationViewModel(
                    sipAddress: x.SipAddress,
                    ip: x.Ip,
                    api: x.Api,
                    userAgent: x.UserAgent,
                    nrOfInputs: x.NrOfInputs,
                    nrOfOutputs: 2,
                    nrOfHeadphones: x.NrOfInputs,
                    nrOfGpis: x.NrOfGpos,
                    nrOfGpos: x.NrOfGpos);
            }).ToList();

        }
    }
}